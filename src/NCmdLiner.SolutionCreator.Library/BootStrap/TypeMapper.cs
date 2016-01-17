using System.Collections.Generic;
using AutoMapper;
using NCmdLiner.SolutionCreator.Library.Common;

namespace NCmdLiner.SolutionCreator.Library.BootStrap
{
    [Singleton]
    public class TypeMapper : ITypeMapper
    {
        private readonly IEnumerable<ITypeMapperConfiguration> _typeMapperConfigurations;

        private bool _isConfigured = false;

        public TypeMapper(IEnumerable<ITypeMapperConfiguration> typeMapperConfigurations)
        {
            _typeMapperConfigurations = typeMapperConfigurations;
        }

        public T Map<T>(object source)
        {
            if(!_isConfigured)
            {
                ConfigureTypeMappers();
            }
            return Mapper.Map<T>(source);
        }

        private void ConfigureTypeMappers()
        {
            foreach (var typeMapperConfiguration in _typeMapperConfigurations)
            {
                typeMapperConfiguration.Configure();
            }
            _isConfigured = true;
        }
    }
}