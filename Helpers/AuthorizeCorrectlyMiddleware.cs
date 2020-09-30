﻿using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

public class AuthorizeCorrectlyMiddleware
{
    readonly RequestDelegate next;

    public AuthorizeCorrectlyMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        await next(context);

        if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
        {
                //the user is authenticated, yet we are returning a 401
                //let's return a 403 instead
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            
        }
    }
}