﻿using Prism.Ioc;
using Vulcanova.Features.Homework.HomeworkDetails;

namespace Vulcanova.Features.Homework;

public static class Config
{
    public static void RegisterHomework(this IContainerRegistry container)
    {
        container.RegisterForNavigation<HomeworkView>();
        container.RegisterForNavigation<HomeworkDetailsView, HomeworkDetailsViewModel>();

        container.RegisterScoped<IHomeworkRepository, HomeworkRepository>();
        container.RegisterScoped<IHomeworkService, HomeworkService>();
    }
}