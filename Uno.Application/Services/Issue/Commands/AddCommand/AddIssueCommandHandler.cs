using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Uno.Application.Behaviors.FileValidators.Contracts;
using Uno.Application.Settings;

namespace Uno.Application.Services;

public class AddIssueCommandHandler : IRequestHandler<AddIssueCommand, Response<object>>
{
    private readonly IDbContext _dbContext;
    private readonly IFileValidationServiceFactory _fileValidationServiceFactory;

    public AddIssueCommandHandler(IDbContext dbContext, IFileValidationServiceFactory fileValidationServiceFactory)
    {
        _dbContext = dbContext;
        _fileValidationServiceFactory = fileValidationServiceFactory;
    }


    public async Task<Response<object>> Handle(AddIssueCommand request, CancellationToken cancellationToken)
    {
        var fileValidationResponse = _fileValidationServiceFactory.GetInstance(request.AttachmentType).Validate(request.File);
        if (fileValidationResponse.IsFailure)
            return Response<object>.Error(fileValidationResponse.Message);

        var issue = new Issue
        {
            ProjectId = request.ProjectToken,
            Description = request.Description,
            Subject = request.Subject,
            Reporter = request.Reporter,
            Url = request.ReportUrl,
        };

        var issueAttachment = new IssueAttachment
        {
            Issue = issue,
            Type = request.AttachmentType,
            Name = request.File.FileName,
            Content = await request.File.ConvertToBase64()
        };

        _dbContext.Set<IssueAttachment>().Add(issueAttachment);
        _dbContext.Set<Issue>().Add(issue);
        _dbContext.Set<ConnectorInIssue>().Add(new ConnectorInIssue
        {
            ConnectorId = request.ConnectorId,
            Issue = issue,
            ConnectorMetaData = request.ConnectorMetaData,
            ClientMetaData = request.ClientMetaData,
        });

        var addConnectorInIssueSaveResponse = await _dbContext.SaveChangeResposeAsync(cancellationToken);
        if (addConnectorInIssueSaveResponse.IsFailure)
            throw new Exception(addConnectorInIssueSaveResponse.Message);

        return Response<object>.Success(new { issue.Id });
    }
}
