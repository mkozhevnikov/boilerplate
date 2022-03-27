namespace Boilerplate.MongoDB.Sample;

using Common.Data;
using global::MongoDB.Driver;
using Models;

public partial class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration) => Configuration = configuration;

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton(CreateClient);
        services.AddSingleton(provider =>
            provider.GetRequiredService<IMongoClient>().GetDatabase("custdb"));
        services.AddTransient<ICollectionContext, CustomerCollectionContext>();
        services.AddTransient<IRepository<Customer, string>, MongoRepository<Customer, string>>();
        services.AddControllers();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseRouting();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}
