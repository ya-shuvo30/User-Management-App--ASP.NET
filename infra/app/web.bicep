param name string
param location string = resourceGroup().location
param tags object = {}

param appServicePlanId string
param appSettings object = {}
param managedIdentity bool = false

resource web 'Microsoft.Web/sites@2022-03-01' = {
  name: name
  location: location
  tags: tags
  identity: managedIdentity ? {
    type: 'SystemAssigned'
  } : null
  properties: {
    serverFarmId: appServicePlanId
    httpsOnly: true
    siteConfig: {
      appSettings: [for key in items(appSettings): {
        name: key.key
        value: key.value
      }]
      netFrameworkVersion: 'v8.0'
      use32BitWorkerProcess: false
      ftpsState: 'FtpsOnly'
      minTlsVersion: '1.2'
    }
  }
}

output defaultHostName string = web.properties.defaultHostName
output id string = web.id
output name string = web.name
output principalId string = managedIdentity ? web.identity.principalId : ''
