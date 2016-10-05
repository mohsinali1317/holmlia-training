<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="Holmlia.Training" generation="1" functional="0" release="0" Id="c260e384-3b77-48cc-af99-3a0cd27fca65" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="Holmlia.TrainingGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="Holmlia.TrainingWeb:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/Holmlia.Training/Holmlia.TrainingGroup/LB:Holmlia.TrainingWeb:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="Holmlia.TraingingWorker:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/Holmlia.Training/Holmlia.TrainingGroup/MapHolmlia.TraingingWorker:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="Holmlia.TraingingWorker:objectStorage" defaultValue="">
          <maps>
            <mapMoniker name="/Holmlia.Training/Holmlia.TrainingGroup/MapHolmlia.TraingingWorker:objectStorage" />
          </maps>
        </aCS>
        <aCS name="Holmlia.TraingingWorkerInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/Holmlia.Training/Holmlia.TrainingGroup/MapHolmlia.TraingingWorkerInstances" />
          </maps>
        </aCS>
        <aCS name="Holmlia.TrainingWeb:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/Holmlia.Training/Holmlia.TrainingGroup/MapHolmlia.TrainingWeb:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="Holmlia.TrainingWeb:objectStorage" defaultValue="">
          <maps>
            <mapMoniker name="/Holmlia.Training/Holmlia.TrainingGroup/MapHolmlia.TrainingWeb:objectStorage" />
          </maps>
        </aCS>
        <aCS name="Holmlia.TrainingWebInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/Holmlia.Training/Holmlia.TrainingGroup/MapHolmlia.TrainingWebInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:Holmlia.TrainingWeb:Endpoint1">
          <toPorts>
            <inPortMoniker name="/Holmlia.Training/Holmlia.TrainingGroup/Holmlia.TrainingWeb/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapHolmlia.TraingingWorker:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/Holmlia.Training/Holmlia.TrainingGroup/Holmlia.TraingingWorker/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapHolmlia.TraingingWorker:objectStorage" kind="Identity">
          <setting>
            <aCSMoniker name="/Holmlia.Training/Holmlia.TrainingGroup/Holmlia.TraingingWorker/objectStorage" />
          </setting>
        </map>
        <map name="MapHolmlia.TraingingWorkerInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/Holmlia.Training/Holmlia.TrainingGroup/Holmlia.TraingingWorkerInstances" />
          </setting>
        </map>
        <map name="MapHolmlia.TrainingWeb:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/Holmlia.Training/Holmlia.TrainingGroup/Holmlia.TrainingWeb/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapHolmlia.TrainingWeb:objectStorage" kind="Identity">
          <setting>
            <aCSMoniker name="/Holmlia.Training/Holmlia.TrainingGroup/Holmlia.TrainingWeb/objectStorage" />
          </setting>
        </map>
        <map name="MapHolmlia.TrainingWebInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/Holmlia.Training/Holmlia.TrainingGroup/Holmlia.TrainingWebInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="Holmlia.TraingingWorker" generation="1" functional="0" release="0" software="C:\Source\Holmlia-Training\Holmlia.Training\csx\Debug\roles\Holmlia.TraingingWorker" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="-1" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="objectStorage" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;Holmlia.TraingingWorker&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;Holmlia.TraingingWorker&quot; /&gt;&lt;r name=&quot;Holmlia.TrainingWeb&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/Holmlia.Training/Holmlia.TrainingGroup/Holmlia.TraingingWorkerInstances" />
            <sCSPolicyUpdateDomainMoniker name="/Holmlia.Training/Holmlia.TrainingGroup/Holmlia.TraingingWorkerUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/Holmlia.Training/Holmlia.TrainingGroup/Holmlia.TraingingWorkerFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="Holmlia.TrainingWeb" generation="1" functional="0" release="0" software="C:\Source\Holmlia-Training\Holmlia.Training\csx\Debug\roles\Holmlia.TrainingWeb" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="-1" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="objectStorage" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;Holmlia.TrainingWeb&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;Holmlia.TraingingWorker&quot; /&gt;&lt;r name=&quot;Holmlia.TrainingWeb&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/Holmlia.Training/Holmlia.TrainingGroup/Holmlia.TrainingWebInstances" />
            <sCSPolicyUpdateDomainMoniker name="/Holmlia.Training/Holmlia.TrainingGroup/Holmlia.TrainingWebUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/Holmlia.Training/Holmlia.TrainingGroup/Holmlia.TrainingWebFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="Holmlia.TrainingWebUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyUpdateDomain name="Holmlia.TraingingWorkerUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="Holmlia.TraingingWorkerFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="Holmlia.TrainingWebFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="Holmlia.TraingingWorkerInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="Holmlia.TrainingWebInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="96a58788-8ae9-420b-a196-50a62e31440d" ref="Microsoft.RedDog.Contract\ServiceContract\Holmlia.TrainingContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="9d19843b-23cb-4523-9100-a6521a2817ff" ref="Microsoft.RedDog.Contract\Interface\Holmlia.TrainingWeb:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/Holmlia.Training/Holmlia.TrainingGroup/Holmlia.TrainingWeb:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>