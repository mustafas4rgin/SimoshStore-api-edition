using Microsoft.AspNetCore.Authentication.Cookies;
using SimoshStore;

public static class BusinessServiceRegistration
{
    public static IServiceCollection AddBusinessService(this IServiceCollection services)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.Cookie.Name ="auth-cookie";
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
            options.SlidingExpiration = true;
            options.LoginPath ="/Login";
            options.LogoutPath ="/Logout";
            options.AccessDeniedPath = "/AccessDenied";
        });

        services.AddHttpContextAccessor();

        
        services.AddTransient<IProductService, ProductService>();
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IAuthRepository, AuthRepository>(); 
        services.AddTransient<IEmailService, SmtpEmailService>();
        services.AddTransient<IDataRepository, DataRepository>();
        services.AddTransient<ICategoryService, CategoryService>();
        services.AddTransient<IBlogService, BlogService>();
        services.AddTransient<IBlogCategoryService, BlogCategoryService>();
        services.AddTransient<ITagService, TagService>();
        services.AddTransient<IProfileService, ProfileService>();
        services.AddTransient<IOrderService, OrderService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<ICommentService, CommentService>();
        return services;
    }
}
