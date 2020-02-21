pipeline {
  agent any
  stages {
    stage('Build Projects') {
      steps {
        sh '''for i in **/MLEM*.csproj; do
    dotnet build $i
done'''
        sh 'dotnet build **/Demos.csproj'
      }
    }

    stage('Pack') {
      steps {
        sh 'find . -type f -name \'*.nupkg\' -delete'
        sh '''for i in **/MLEM*.csproj; do
    dotnet pack $i --version-suffix ${BUILD_NUMBER}
done'''
      }
    }

    stage('Publish') {
      steps {
        sh '''for i in **/*.nupkg; do
    nuget push $i -Source https://nuget.ellpeck.de/v3/index.json
done'''
      }
    }

  }
}