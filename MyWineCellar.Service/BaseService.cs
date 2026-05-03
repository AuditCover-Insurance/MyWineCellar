using Microsoft.AspNetCore.Http;
using MyWineCellar.Data;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace MyWineCellar.Service;

public class BaseService(WineCellarDbContext db, IHttpContextAccessor httpContextAccessor)
{
    protected WineCellarDbContext Db => db;
    protected IHttpContextAccessor HttpContextAccessor => httpContextAccessor;

    protected int UserId => int.Parse(httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
