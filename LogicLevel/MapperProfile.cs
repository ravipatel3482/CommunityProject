using AutoMapper;
using ProjectDataStructure.Electronics;
using ProjectDataStructure.IndiaViewModel;

namespace LogicLevel
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            //CreateMap<Employee, EmployeeViewModel>();

            //CreateMap<EmployeeViewModel, Employee>()
            //    .ForPath(dest => dest.Department.DepartmentId,
            //               opt => opt.MapFrom(src => src.DepartmentId))
            //    .ForPath(dest => dest.Department.DepartmentName,
            //               opt => opt.MapFrom(src => src.DepartmentName));

            //CreateMap<SaveEmployeeModel, Employee>();
            //CreateMap<EditEmployeeViewModel, Employee>();
            CreateMap<ServicesOrderViewModel, ServicesOrder>();

        }
    }
}
