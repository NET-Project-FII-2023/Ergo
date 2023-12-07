namespace Ergo.Domain.Common
{
    public static class Utils
    {
        public static string GenerateBranchId()
        {
            return $"TASK-{Guid.NewGuid()}";
        }
    }
}
