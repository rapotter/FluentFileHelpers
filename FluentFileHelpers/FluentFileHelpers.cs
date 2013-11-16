using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentFileHelpers
{
    public class FluentFileHelpers
    {
        public ISet<MapTypes> MapTypes { get; set; }

        public static FluentFileHelpers Configure()
        {
            FluentFileHelpers Configuration = new FluentFileHelpers();
            return Configuration;
        }

        public FluentFileHelpers AddFromAssemblyOf<T>()
        {
            var Types = typeof(T).Assembly.GetTypes();
            var GenericTypes = Types.Where(x => x.BaseType != null && x.BaseType.IsGenericType);
            var ClassMapTypes = GenericTypes.Where(x => x.BaseType.GetGenericTypeDefinition() == typeof(ClassMap<>));
            var SubClassMapTypes = GenericTypes.Where(x => x.BaseType.GetGenericTypeDefinition() == typeof(SubClassMap<>));

            MapTypes = new HashSet<MapTypes>(
                                    ClassMapTypes
                                        .GroupJoin(
                                            Types,
                                            x => x.BaseType.GenericTypeArguments.First(),
                                            y => y.BaseType,
                                            (x, z) => new MapTypes
                                            {
                                                ClassMapType = x,
                                                ClassMapGenericType = x.BaseType.GenericTypeArguments.First(),
                                                ClassMap = Activator.CreateInstance(x),
                                                SubClassMapTypes = z.Join(
                                                    SubClassMapTypes,
                                                    w => w,
                                                    y => y.BaseType.GenericTypeArguments.First(),
                                                    (w, y) => new MapTypes
                                                    {
                                                        ClassMapType = y,
                                                        ClassMapGenericType = w,
                                                        ClassMap = Activator.CreateInstance(y)
                                                    }
                                                )
                                                .ToList()
                                            }
                                        )
                        );

            return this;
        }

        public T Map<T>(string s)
        {
            MapTypes MapType = MapTypes
                                .SingleOrDefault(x => x.ClassMapGenericType == typeof(T));

            if (MapType.SubClassMapTypes.Count > 0)
            {
                MapTypes SubClassMapType = MapType.SubClassMapTypes.SingleOrDefault(x => x.ClassMap.Discriminator == MapType.ClassMap.Discriminator.GetDiscriminatorValueFrom(s));
                dynamic item = Activator.CreateInstance(SubClassMapType.ClassMapGenericType);
                return SubClassMapType.ClassMap.Assign(MapType.ClassMap.Assign(item, s), s);
            }
            else
            {
                return MapType.ClassMap.Assign(s);
            }
        }

        public ClassMap<T> GetClassMap<T>() where T : new()
        {
            MapTypes FileClassMapType = MapTypes.SingleOrDefault(x => x.ClassMapGenericType == typeof(T));
            if (FileClassMapType != null)
            {
                return FileClassMapType.ClassMap;
            }
            else
            {
                MapTypes FileSubClassMapType = MapTypes.SelectMany(x => x.SubClassMapTypes).SingleOrDefault(x => x.ClassMapGenericType == typeof(T));
                if (FileSubClassMapType != null)
                {
                    return FileSubClassMapType.ClassMap;
                }
            }
            return null;
        }

    }

}
