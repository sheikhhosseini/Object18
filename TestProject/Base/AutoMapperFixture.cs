using AutoMapper;
using Core.Modules.AccountModule.Dtos;

namespace TestProject.Base;

public class AutoMapperFixture
{
    public IMapper Mapper;

    public AutoMapperFixture()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(
                typeof(RegisterDto).Assembly
            );
        });

        Mapper = config.CreateMapper();
    }
}