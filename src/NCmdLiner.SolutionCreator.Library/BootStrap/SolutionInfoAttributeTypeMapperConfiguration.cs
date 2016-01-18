using AutoMapper;
using NCmdLiner.SolutionCreator.Library.Services;
using NCmdLiner.SolutionCreator.Library.ViewModels;

namespace NCmdLiner.SolutionCreator.Library.BootStrap
{
    public class SolutionInfoAttributeTypeMapperConfiguration : ITypeMapperConfiguration
    {
        public void Configure()
        {
            Mapper.CreateMap<SolutionInfoAttribute, SolutionInfoAttributeViewModel>();
            Mapper.CreateMap<SolutionInfoAttributeViewModel, SolutionInfoAttribute>();
        }
    }
}