require "albacore" 
require_relative "filesystem"
require_relative "gallio-task"

reportsPath = "reports"
version = ENV["BUILD_NUMBER"]

task :build => :pushLocalPackage
task :deploy => :pushPublicPackage

assemblyinfo :assemblyInfo do |asm|
    asm.version = version
    asm.company_name = "Ultraviolet Catastrophe"
    asm.product_name = "Bender"
    asm.title = "Bender"
    asm.description = "Xml and json de/serialization for .NET"
    asm.copyright = "Copyright (c) #{Time.now.year} Ultraviolet Catastrophe"
    asm.output_file = "src/Bender/Properties/AssemblyInfo.cs"
end

msbuild :buildLibrary => :assemblyInfo do |msb|
    msb.properties :configuration => :Release
    msb.targets :Clean, :Build
    msb.solution = "src/Bender/Bender.csproj"
end

msbuild :buildTests => :buildLibrary do |msb|
    msb.properties :configuration => :Release
    msb.targets :Clean, :Build
    msb.solution = "src/Tests/Tests.csproj"
end

task :unitTestInit do
	FileSystem.EnsurePath(reportsPath)
end

gallio :unitTests => [:buildTests, :unitTestInit] do |runner|
	runner.echo_command_line = true
	runner.add_test_assembly("src/Tests/bin/Release/Tests.dll")
	runner.verbosity = 'Normal'
	runner.report_directory = reportsPath
	runner.report_name_format = 'tests'
	runner.add_report_type('Html')
end

nugetApiKey = ENV["NUGET_API_KEY"]
deployPath = "deploy"
artifactsPath = 'artifacts'

packagePath = File.join(deployPath, "package")
nuspecFilename = "Bender.nuspec"
packageLibPath = File.join(packagePath, "lib")
binPath = "src/Bender/bin/release"
packageFilePath = File.join(deployPath, "Bender.#{version}.nupkg")

task :prepPackage => :unitTests do
	FileSystem.DeleteDirectory(deployPath)
	FileSystem.EnsurePath(packageLibPath)
	FileSystem.DeleteDirectory(artifactsPath)
    FileSystem.EnsurePath(artifactsPath)
	FileSystem.CopyFiles(File.join(binPath, "Bender.dll"), packageLibPath)
	FileSystem.CopyFiles(File.join(binPath, "Bender.pdb"), packageLibPath)
end

nuspec :createSpec => :prepPackage do |nuspec|
   nuspec.id = "Bender"
   nuspec.version = version
   nuspec.authors = "Mike O'Brien"
   nuspec.owners = "Mike O'Brien"
   nuspec.title = "Bender"
   nuspec.description = "Xml and json de/serialization for .NET"
   nuspec.summary = "Xml and json de/serialization for .NET"
   nuspec.language = "en-US"
   nuspec.licenseUrl = "https://github.com/mikeobrien/Bender/blob/master/LICENSE"
   nuspec.projectUrl = "https://github.com/mikeobrien/Bender"
   nuspec.iconUrl = "https://github.com/mikeobrien/Bender/raw/master/misc/logo.png"
   nuspec.working_directory = packagePath
   nuspec.output_file = nuspecFilename
   nuspec.tags = "xml json serialization deserialization deserializer serializer"
end

nugetpack :createPackage => :createSpec do |nugetpack|
   nugetpack.nuspec = File.join(packagePath, nuspecFilename)
   nugetpack.base_folder = packagePath
   nugetpack.output = deployPath
end

task :pushLocalPackage => :createPackage do
	FileSystem.CopyFiles(packageFilePath, artifactsPath)
end

nugetpush :pushPublicPackage => :createPackage do |nuget|
    nuget.apikey = nugetApiKey
    nuget.package = packageFilePath.gsub('/', '\\')
end