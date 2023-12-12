﻿using Ergo.App.Services.Responses;
using Ergo.App.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ergo.App.Contracts
{
    public interface ITaskDataService
    {
        Task<List<TaskViewModel>> GetTasksAsync();
        Task<List<TaskViewModel>> GetTasksByProjectIdAsync(Guid projectId);

        Task<ApiResponse<TaskDto>> CreateTaskAsync(TaskViewModel taskViewModel);
    }
}