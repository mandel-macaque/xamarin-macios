<#
    .SYNOPSIS
        Cancels the pipeline and no other steps of job will be executed.
#>
function Stop-Pipeline {
    # we can achive the cancelation of the pipeline setting the following env variables
    Write-Host "##vso[task.setvariable variable=agent.jobstatus;]canceled"
    Write-Host "##vso[task.complete result=Canceled;]DONE"
}

Export-ModuleMember -Function Stop-Pipeline