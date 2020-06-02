<#
VSTS interaction unit tests.
#>

Import-Module ./VSTS -Force

Describe 'Stop-Pipeline' {
    Context 'default' {
        It 'sets the correct variables' {
            Mock Write-Host

            Stop-Pipeline

            Assert-MockCalled -CommandName Write-Host -Times 1 -Scope It -ParameterFilter { $Object -eq "##vso[task.setvariable variable=agent.jobstatus;]canceled" }
            Assert-MockCalled -CommandName Write-Host -Times 1 -Scope It -ParameterFilter { $Object -eq "##vso[task.complete result=Canceled;]DONE" }
        }
    }
}
