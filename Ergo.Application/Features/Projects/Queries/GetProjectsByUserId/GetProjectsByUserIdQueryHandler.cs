﻿using Ergo.Application.Persistence;
using Ergo.Domain.Entities;
using MediatR;

namespace Ergo.Application.Features.Projects.Queries.GetProjectsByUserId
{
    public class GetProjectsByUserIdQueryHandler : IRequestHandler<GetProjectsByUserIdQuery, GetProjectsByUserIdQueryResponse>
    {
        private readonly IProjectRepository projectRepository;

        public GetProjectsByUserIdQueryHandler(IProjectRepository projectRepository)
        {
            this.projectRepository = projectRepository;
        }

        public async Task<GetProjectsByUserIdQueryResponse> Handle(GetProjectsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var projects = await projectRepository.GetProjectsByUserId(Guid.Parse(request.UserId));
            if (!projects.IsSuccess)
            {
                return new GetProjectsByUserIdQueryResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { projects.Error }
                };
            }
            return new GetProjectsByUserIdQueryResponse
            {
                Success = true,
                Projects = projects.Value.Select(p => new ProjectDto
                {
                    ProjectId = Guid.Parse(p.ProjectId.ToString()),
                    ProjectName = p.ProjectName,
                    Description = p.Description,
                    StartDate = p.StartDate,
                    Deadline = p.Deadline,
                    State = p.State,
                    GitRepository = p.GitRepository,
                    Members = p.Members
                }).ToList()
            };
        }
    }
}
