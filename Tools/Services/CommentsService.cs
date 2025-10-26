﻿using Microsoft.Extensions.DependencyInjection; 

namespace coer91.Tools
{
    public static class CommentsService
    {
        public static IServiceCollection AddComments(this IServiceCollection context)
        {
            context.AddSwaggerGen(setup => {
                foreach (string xmlPath in Directory.EnumerateFiles(AppContext.BaseDirectory, "*.xml"))
                    setup.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);                
            });

            return context;
        }
    }
}