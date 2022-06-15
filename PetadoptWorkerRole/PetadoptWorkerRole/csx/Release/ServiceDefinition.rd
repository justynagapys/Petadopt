<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" name="PetadoptWorkerRole" generation="1" functional="0" release="0" Id="4c8fac60-fc9b-46f9-a05d-b0f5444f59f2" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="PetadoptWorkerRoleGroup" generation="1" functional="0" release="0">
      <settings>
        <aCS name="NewPet:APPINSIGHTS_INSTRUMENTATIONKEY" defaultValue="">
          <maps>
            <mapMoniker name="/PetadoptWorkerRole/PetadoptWorkerRoleGroup/MapNewPet:APPINSIGHTS_INSTRUMENTATIONKEY" />
          </maps>
        </aCS>
        <aCS name="NewPet:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/PetadoptWorkerRole/PetadoptWorkerRoleGroup/MapNewPet:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="NewPetInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/PetadoptWorkerRole/PetadoptWorkerRoleGroup/MapNewPetInstances" />
          </maps>
        </aCS>
      </settings>
      <maps>
        <map name="MapNewPet:APPINSIGHTS_INSTRUMENTATIONKEY" kind="Identity">
          <setting>
            <aCSMoniker name="/PetadoptWorkerRole/PetadoptWorkerRoleGroup/NewPet/APPINSIGHTS_INSTRUMENTATIONKEY" />
          </setting>
        </map>
        <map name="MapNewPet:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/PetadoptWorkerRole/PetadoptWorkerRoleGroup/NewPet/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapNewPetInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/PetadoptWorkerRole/PetadoptWorkerRoleGroup/NewPetInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="NewPet" generation="1" functional="0" release="0" software="C:\Users\justy\OneDrive\Pulpit\Petadopt\PetadoptWorkerRole\PetadoptWorkerRole\csx\Release\roles\NewPet" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="-1" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="APPINSIGHTS_INSTRUMENTATIONKEY" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;NewPet&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;NewPet&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/PetadoptWorkerRole/PetadoptWorkerRoleGroup/NewPetInstances" />
            <sCSPolicyUpdateDomainMoniker name="/PetadoptWorkerRole/PetadoptWorkerRoleGroup/NewPetUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/PetadoptWorkerRole/PetadoptWorkerRoleGroup/NewPetFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="NewPetUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="NewPetFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="NewPetInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
</serviceModel>