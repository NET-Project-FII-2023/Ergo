using Ergo.Api.Controllers;
using Ergo.Api.Models;
using Ergo.Application.Features.Projects.Queries.GetProjectGithubData;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Octokit;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/v1/GitHub")]
    public class GitHubController : ApiControllerBase
    {
        private readonly GitHubClient gitHubClient;

        public GitHubController()
        {
            gitHubClient = new GitHubClient(new ProductHeaderValue("Ergo"));
        }

        [HttpPost]
        [Route("commits")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCommitsFromBranch(GetProjectGithubDataQuery command, string Branch)
        {
            try
            {
                var result = await Mediator.Send(command);
                if (!result.Success)
                {
                    return BadRequest(result);
                }
                gitHubClient.Credentials = new Credentials(result.GithubToken);

                IReadOnlyList<GitHubCommit> commits = await gitHubClient.Repository.Commit
                    .GetAll(result.ProjectOwner, result.ProjectRepository, new CommitRequest { Sha = Branch });

                List<GitHubCommitDto> commitNames = new List<GitHubCommitDto>();

                foreach (var commit in commits)
                {
                    commitNames.Add(new GitHubCommitDto
                    {
                        CommitName = commit.Commit.Message,
                        Url = commit.Commit.Url,
                        Author = commit.Commit.Author.Name,
                        Date = commit.Commit.Author.Date.ToString()
                    });
                }

                return Ok(commitNames);
            }
            catch (NotFoundException ex)
            {
                return NotFound($"Repository or branch not found: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [HttpPost]
        [Route("branches")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBranches(GetProjectGithubDataQuery command)
        {
            try
            {
                var result = await Mediator.Send(command);
                if (!result.Success)
                {
                    return BadRequest(result);
                }
                gitHubClient.Credentials = new Credentials(result.GithubToken);
                IReadOnlyList<Branch> branches = await gitHubClient.Repository.Branch.GetAll(result.ProjectOwner, result.ProjectRepository);
                //only the name of the branches
                List<string> branchNames = new List<string>();
                foreach (var branch in branches)
                {
                    branchNames.Add(branch.Name);
                }
                return Ok(branchNames);
              
            }
            catch (NotFoundException ex)
            {
                return NotFound($"Repository not found: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
