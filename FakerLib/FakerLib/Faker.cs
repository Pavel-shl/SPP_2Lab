using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using FakerLib.Generator;

namespace FakerLib
{
    public class Faker
    {
        //хранят все возможные генераторы
        private Dictionary<Type, IGenerator> Generators = new Dictionary<Type, IGenerator>();
        //хранят все возможные коллекции
        private Dictionary<Type, ICollectionGenerator> CollectionsGenerators = new Dictionary<Type, ICollectionGenerator>();


        Stack<Type> nestedTypes = new Stack<Type>();

        public Faker()
        {
            //типы в перменную сохраняем
            Type mainInterface = typeof(IGenerator),
                 collectionsInterface = typeof(ICollectionGenerator);
            try
            {
                //текущая сборка получение
                Assembly assembly = Assembly.GetExecutingAssembly();
                //хранит все типы этой сборки
                Type[] types = assembly.GetTypes();

                for (int i = 0; i < types.Length; i++)
                {
                    //если они совместимы с интервейсом
                    if (mainInterface.IsAssignableFrom(types[i]) && mainInterface != types[i])
                    {
                        //создаем сущность класса и добавляем в наш массивчик генератора
                        IGenerator generator = (IGenerator)Activator.CreateInstance(types[i]);
                        Generators.Add(generator.ResultType, generator);
                        continue;
                    }
                    //то же самое но для коллекций
                    if (collectionsInterface.IsAssignableFrom(types[i]) && collectionsInterface != types[i])
                    {

                        ICollectionGenerator collectionGenerator = (ICollectionGenerator)Activator.CreateInstance(types[i]);
                        foreach (Type key in collectionGenerator.CollectionType)
                        {
                            CollectionsGenerators.Add(key, collectionGenerator);
                        }
                        continue;
                    }

                }
            }
            catch
            {
                throw new Exception("Cannot create Faker");
            }

        }

        // передаешь тип и она создает все с созданными полями
        public T Create<T>()
        {
            Type type = typeof(T);
            //проверка вложенности типов 
            nestedTypes.Push(type);

            //передали тип для которого есть генератор уже
            object tmp = CheckGenerators(type);
            if (tmp != null)
            {
                nestedTypes.Pop();
                return (T)tmp;
            }

            //null
            T result = default(T);


            try
            {
                //если есть конструктор без параметров
                result = Activator.CreateInstance<T>();
            }
            catch (MissingMethodException)
            {
                //если нет конструктора без параметров
                //получаем все конструкторы
                var typeConstructors = type.GetConstructors();
                if (typeConstructors.Count() == 0)
                {
                    nestedTypes.Pop();
                    return result;
                }
                // смотрим все конструкторы
                foreach (ConstructorInfo constructor in typeConstructors)
                {
                    //получаем все параметры конструктора
                    List<object> values = new List<object>();
                    foreach (ParameterInfo parameter in constructor.GetParameters())
                    { 
                        values.Add(GenerateValue(parameter.ParameterType));
                    }

                    result = (T)constructor.Invoke(values.ToArray());
                }

            }
            catch (Exception ex)
            {
                //если не тот exeption
                nestedTypes.Pop();
                throw ex;
            }

            //если примитивы
            if (type.IsValueType)

            {
                nestedTypes.Pop();
                return result;
            }

            //просматриваем все поля этого типа открытые
            foreach (FieldInfo f in type.GetFields())
            {
                //и записываем в них фейковые значения
                object fieldValue = GenerateValue(f.FieldType);
                f.SetValue(result, fieldValue);
            }

            foreach (PropertyInfo property in type.GetProperties())
            {
                //то же самое только со свойствами
                if (property.CanWrite)
                {
                    object propertyValue = GenerateValue(property.PropertyType);
                    property.SetValue(result, propertyValue);
                }
            }
            nestedTypes.Pop();
            return result;
        }


        private object CheckGenerators(Type destinationType)
        {
            //смотрит все типы 
            foreach (Type t in Generators.Keys)
            {
                if (Generators[t].ResultType == destinationType)
                {
                    //генерируем и возвращаем значение
                    return Generators[t].Generate();
                }

            }
            // so for generic и смотри еще в дженереках которые могут быть вложенными
            if (!destinationType.IsGenericType)
                return null;
            foreach (Type t in CollectionsGenerators.Keys)
            {
                if (CollectionsGenerators[t].CollectionType.Contains(destinationType.GetGenericTypeDefinition()))
                {
                    var type = CollectionsGenerators[t].GetType();
                    var Method = type.GetMethod("Generate");
                    var MemberType = destinationType.GenericTypeArguments.First();
                    var res = (object)Method.MakeGenericMethod(MemberType).Invoke(CollectionsGenerators[t], new object[] { this });
                    return res;
                }

            }

            return null;
        }

        private object GenerateValue(Type valueType)
        {
            object value = default(object);
            if (Generators.ContainsKey(valueType))
            {
                value = Generators[valueType].Generate();
            }
            else if (!nestedTypes.Contains(valueType))
            {
                value = this.GetType().GetMethod("Create").MakeGenericMethod(valueType).Invoke(this, null);
            }

            return value;

        }
    }
}
