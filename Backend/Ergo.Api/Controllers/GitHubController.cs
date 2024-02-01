using Ergo.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Octokit;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/v1/GitHub")]
    public class GitHubController : ControllerBase
    {
        private readonly GitHubClient gitHubClient;

        public GitHubController()
        {
            gitHubClient = new GitHubClient(new ProductHeaderValue("Ergo"));
        }

        [HttpGet("commits")]
        public async Task<IActionResult> GetCommitsFromBranch(
            [FromQuery] string owner,
            [FromQuery] string repo,
            [FromQuery] string branch)
        {
            try
            {
                gitHubClient.Credentials = new Credentials("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");

                IReadOnlyList<GitHubCommit> commits = await gitHubClient.Repository.Commit
                    .GetAll(owner, repo, new CommitRequest { Sha = branch });

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
    }
}
