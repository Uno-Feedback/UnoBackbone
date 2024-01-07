namespace Uno.Infrastructure.ExternalServices.Services.Contracts;
/// <summary>
/// An Interface for recieving Jira Client.
/// </summary>
public interface IJiraClientFactory
{
    public Jira GetJiraClient(JiraConfig jiraConfig);
}
