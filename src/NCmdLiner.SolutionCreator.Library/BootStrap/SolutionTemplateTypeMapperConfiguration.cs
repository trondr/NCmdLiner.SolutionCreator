using AutoMapper;
using NCmdLiner.SolutionCreator.Library.Model;
using NCmdLiner.SolutionCreator.Library.ViewModels;

namespace NCmdLiner.SolutionCreator.Library.BootStrap
{
    public class SolutionTemplateTypeMapperConfiguration : ITypeMapperConfiguration
    {
        public void Configure()
        {
            Mapper.CreateMap<SolutionTemplate, SolutionTemplateViewModel>();
            Mapper.CreateMap<SolutionTemplateViewModel, SolutionTemplate>();
        }
    }
}