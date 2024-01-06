using Uno.Infrastructer.ExternalServices.Dtos;
using Uno.Infrastructer.ExternalServices.Services.Contracts;

namespace Uno.Infrastructer.ExternalServices.Services;

public class JiraClientFactory : IJiraClientFactory
{
    private static object _lock = new();
    private Jira _jiraClient = null;
    public Jira GetJiraClient(JiraConfig jiraConfig)
    {
        lock (_lock)
        {
            if (_jiraClient == null)
                _jiraClient = Jira.CreateRestClient(jiraConfig.Url, jiraConfig.UserName, jiraConfig.Password);

            return _jiraClient;
        }
    }
}
