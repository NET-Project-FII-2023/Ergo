using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            gitHubClient.Credentials = new Credentials("va-faceti-un-token-de-github-si-il-bagati-aici");
        }

        [HttpGet("commits")]
        public async Task<IActionResult> GetCommitsFromBranch(
            [FromQuery] string owner,
            [FromQuery] string repo,
            [FromQuery] string branch)
        {
            try
            {
                IReadOnlyList<GitHubCommit> commits = await gitHubClient.Repository.Commit
                    .GetAll(owner, repo, new CommitRequest { Sha = branch });

                List<string> commitNames = new List<string>();

                foreach (var commit in commits)
                {
                    commitNames.Add(commit.Commit.Message);
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
