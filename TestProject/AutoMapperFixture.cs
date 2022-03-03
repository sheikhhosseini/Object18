using AutoMapper;
using Core.Modules.Account.Dtos;

namespace TestProject;

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