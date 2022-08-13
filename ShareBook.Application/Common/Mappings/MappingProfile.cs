using System.Reflection;
using AutoMapper;

namespace ShareBook.Application.Common.Mappings;

public class MappingProfile : AutoMapper.Profile
{
	public MappingProfile()
		=> ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
	private void ApplyMappingsFromAssembly(Assembly assembly)
	{
		var types = assembly
		                 .GetExportedTypes()
		                 .Where(t => t.GetInterfaces()
		                              .Any(i => i.IsGenericType
		                                        && i.GetGenericTypeDefinition() == typeof (IMapFrom<>)))
		                 .ToList();
		
		foreach (var type in types)
		{
			var instance = Activator.CreateInstance(type);

			MethodInfo method = type
				.GetMethod("Mapping")
			             ?? type.GetInterface("IMapFrom`1")
			                    ?.GetMethod("Mapping");
			
			method!.Invoke(instance, new object[] { this });
		}
	}
}