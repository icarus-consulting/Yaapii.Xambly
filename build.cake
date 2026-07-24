#tool nuget:?package=OpenCover&version=4.7.922
#tool nuget:?package=Codecov&version=1.12.3
#addin nuget:?package=Cake.Figlet&version=1.3.1
#addin nuget:?package=Cake.Incubator&version=5.1.0

var target                  = Argument("target", "Default");
var configuration           = "Release";

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////
// We define where the build artifacts should be places
// this is relative to the project root folder
var buildArtifacts          = Directory("./artifacts");
var deployment              = Directory("./artifacts/deployment");
var version                 = "1.2.1";

///////////////////////////////////////////////////////////////////////////////
// MODULES
///////////////////////////////////////////////////////////////////////////////
var modules                 = Directory("./src");
// To skip building a project in the source folder add the project folder name
// as string to the list e.g. "Yaapii.Atoms".
var excludedModules         = new List<string>() { };

// Unit tests
var unitTests               = Directory("./tests");
// To skip executing a test in the tests folder add the test project folder name
// as string to the list e.g. "Yaapii.IrrelevantTests".
var excludedUnitTests        = new List<string>() { };

///////////////////////////////////////////////////////////////////////////////
// CONFIGURATION VARIABLES
///////////////////////////////////////////////////////////////////////////////
var isAppVeyor              = AppVeyor.IsRunningOnAppVeyor;
var isWindows               = IsRunningOnWindows();

// For GitHub release
var owner                   = "icarus-consulting";
var repository              = "Yaapii.Xambly";

// For NuGetFeed
var nuGetSource             = "https://api.nuget.org/v3/index.json";
var appVeyorNuGetFeed       = "https://ci.appveyor.com/nuget/icarus/api/v2/package";

// API key tokens for deployment
var gitHubToken             = "";
var nugetReleaseToken       = "";
var appVeyorFeedToken       = "";
var codeCovToken            = "";

void RunCommand(string fileName, string arguments)
{
    var startInfo = new System.Diagnostics.ProcessStartInfo();
    startInfo.FileName = fileName;
    startInfo.Arguments = arguments;
    startInfo.WorkingDirectory = MakeAbsolute(Directory("./")).FullPath;
    startInfo.UseShellExecute = false;
    startInfo.RedirectStandardOutput = true;
    startInfo.RedirectStandardError = true;
    startInfo.CreateNoWindow = true;

    using (var process = new System.Diagnostics.Process())
    {
        process.StartInfo = startInfo;
        process.Start();
        var stdout = process.StandardOutput.ReadToEnd();
        var stderr = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (!string.IsNullOrWhiteSpace(stdout))
        {
            Information(stdout.TrimEnd());
        }

        if (!string.IsNullOrWhiteSpace(stderr))
        {
            Information(stderr.TrimEnd());
        }

        if (process.ExitCode != 0)
        {
            throw new Exception($"{fileName} {arguments} failed with exit code {process.ExitCode}");
        }
    }
}

string FirstTool(string pattern)
{
    return GetFiles(pattern).First().FullPath;
}

string DotNetExecutable()
{
    var configured = EnvironmentVariable("DOTNET_EXE");
    if (!string.IsNullOrWhiteSpace(configured))
    {
        return configured;
    }

    var programFiles = EnvironmentVariable("ProgramFiles");
    if (!string.IsNullOrWhiteSpace(programFiles))
    {
        var candidate = System.IO.Path.Combine(programFiles, "dotnet", "dotnet.exe");
        if (System.IO.File.Exists(candidate))
        {
            return candidate;
        }
    }

    return "dotnet";
}

void RunOpenCover(string dotNetExecutable, string testProject, string configuration, string outputFile)
{
    var scriptPath = MakeAbsolute(File("./run-opencover.ps1")).FullPath;
    var openCover = FirstTool("./tools/**/OpenCover.Console.exe");
    var workingDirectory = MakeAbsolute(Directory("./")).FullPath;
    RunCommand(
        "powershell",
        $"-NoProfile -ExecutionPolicy Bypass -File \"{scriptPath}\" -OpenCoverExe \"{openCover}\" -DotNetExe \"{dotNetExecutable}\" -TestProject \"{testProject}\" -Configuration \"{configuration}\" -OutputFile \"{outputFile}\" -WorkingDirectory \"{workingDirectory}\""
    );
}

///////////////////////////////////////////////////////////////////////////////
// Version
///////////////////////////////////////////////////////////////////////////////
Task("Version")
.WithCriteria(() => isAppVeyor && BuildSystem.AppVeyor.Environment.Repository.Tag.IsTag)
.Does(() => 
{
    Information(Figlet("Version"));
    
    version = BuildSystem.AppVeyor.Environment.Repository.Tag.Name;
    Information($"Set version to '{version}'");
});

///////////////////////////////////////////////////////////////////////////////
// Clean
///////////////////////////////////////////////////////////////////////////////
Task("Clean")
.Does(() =>
{
    Information(Figlet("Clean"));
    
    CleanDirectories(new DirectoryPath[] { buildArtifacts });
    foreach(var module in GetSubDirectories(modules))
    {
        var name = module.GetDirectoryName();
        if(!excludedModules.Contains(name))
        {
            CleanDirectories(
                new DirectoryPath[] 
                { 
                    $"{module}/bin",
                    $"{module}/obj",
                }
            );
        }
    }
});

///////////////////////////////////////////////////////////////////////////////
// Restore
///////////////////////////////////////////////////////////////////////////////
Task("Restore")
.Does(() =>
{
    Information(Figlet("Restore"));

    RunCommand("dotnet", $"restore \"./{repository}.sln\"");
});

///////////////////////////////////////////////////////////////////////////////
// Build
///////////////////////////////////////////////////////////////////////////////
Task("Build")
.IsDependentOn("Version")
.IsDependentOn("Clean")
.IsDependentOn("Restore")
.Does(() =>
{
    Information(Figlet("Build"));

    var skipped = new List<string>();
    foreach(var module in GetSubDirectories(modules))
    {
        var name = module.GetDirectoryName();
        if(!excludedModules.Contains(name))
        {
            Information($"Building {name}");

            RunCommand("dotnet", $"build \"{module.FullPath}\" --configuration {configuration} --no-restore -p:VersionPrefix={version}");
        }
        else
        {
            skipped.Add(name);
        }
    }
    if (skipped.Count > 0)
    {
        Warning("The following builds have been skipped:");
        foreach(var name in skipped)
        {
            Warning($"  {name}");
        }
    }
});

///////////////////////////////////////////////////////////////////////////////
// Unit Tests
///////////////////////////////////////////////////////////////////////////////
Task("UnitTests")
.IsDependentOn("Build")
.Does(() => 
{
    Information(Figlet("Unit Tests"));

    var settings = 
        new DotNetCoreTestSettings()
        {
            Configuration = configuration,
            NoRestore = true
        };
    var skipped = new List<string>();   
    foreach(var test in GetSubDirectories(unitTests))
    {
        var name = test.GetDirectoryName();
        if(excludedUnitTests.Contains(name))
        {
            skipped.Add(name);
        }
    }
    if (skipped.Count > 0)
    {
        Warning("The following tests have been skipped:");
        foreach(var name in skipped)
        {
            Warning($"  {name}");
        }
    }
});

///////////////////////////////////////////////////////////////////////////////
// Generate Coverage
///////////////////////////////////////////////////////////////////////////////
Task("GenerateCoverage")
.IsDependentOn("Build")
.Does(() => 
{
    Information(Figlet("Generate Coverage"));

    RunOpenCover(DotNetExecutable(), "./tests/Test.Yaapii.Xambly/Test.Yaapii.Xambly.csproj", configuration, $"{buildArtifacts.Path}/coverage.xml");
});

///////////////////////////////////////////////////////////////////////////////
// Upload Coverage
///////////////////////////////////////////////////////////////////////////////
Task("UploadCoverage")
.IsDependentOn("GenerateCoverage")
.IsDependentOn("Credentials")
.WithCriteria(() => isAppVeyor)
.Does(() =>
{
    Information(Figlet("Upload Coverage"));

    var codecov = FirstTool("./tools/**/codecov.exe");
    RunCommand(codecov, $"-f \"{buildArtifacts.Path}/coverage.xml\" -t \"{codeCovToken}\"");
});

///////////////////////////////////////////////////////////////////////////////
// Assert Packages
///////////////////////////////////////////////////////////////////////////////
Task("AssertPackages")
.Does(() => 
{
    Information(Figlet("Assert Packages"));

    foreach (var module in GetSubDirectories(modules))
    {
        var name = module.GetDirectoryName();
        if(!excludedModules.Contains(name))
        {
            var project = ParseProject(new FilePath($"{module}/{name}.csproj"), configuration);
            var packageVersion = new Dictionary<string, string>();
            foreach (var package in project.PackageReferences)
            {
                packageVersion[package.Name] = package.Version;
            }

            foreach (var package in packageVersion)
            {
                if (package.Key.Contains(".Sources"))
                {
                    var nonSourcesPackage = package.Key.Replace(".Sources", string.Empty);
                    if (packageVersion[nonSourcesPackage] != package.Value)
                    {
                        throw new Exception(
                            $"Reference nuget packages must have equal version in project {name}:{Environment.NewLine}"
                            + $"\t{package.Key} {package.Value} and {nonSourcesPackage} {packageVersion[nonSourcesPackage]}.{Environment.NewLine}"
                            + $"\tUpdate nuget package in the {name}.csproj file.{Environment.NewLine}"
                            + $"\tHint: search for '<PackageReference Include=\"{package.Key}\" Version=\"{package.Value}\" Condition=\"'$(Configuration)' == 'ReleaseSources'\">'."
                        );    
                    }
                }
            }
        }
    }
    Information("Package validation passed.");
});

///////////////////////////////////////////////////////////////////////////////
// NuGet
///////////////////////////////////////////////////////////////////////////////
Task("NuGet")
.IsDependentOn("Version")
.IsDependentOn("Clean")
.IsDependentOn("AssertPackages")
.IsDependentOn("Restore")
.IsDependentOn("Build")
.Does(() =>
{
    Information(Figlet("NuGet"));
    Information($"Building NuGet Package for Version {version}");

    foreach (var module in GetSubDirectories(modules))
    {
        var name = module.GetDirectoryName();

        RunCommand("dotnet", $"pack \"{module.FullPath}\" --configuration {configuration} --output \"{buildArtifacts.Path}\" --include-symbols --no-restore -p:VersionPrefix={version} -p:SymbolPackageFormat=snupkg");

        RunCommand("dotnet", $"pack \"{module.FullPath}\" --configuration ReleaseSources --output \"{buildArtifacts.Path}\" -p:VersionPrefix={version} -p:PackageId={name}.Sources -p:IncludeBuildOutput=false");
    }
});

///////////////////////////////////////////////////////////////////////////////
// Credentials
///////////////////////////////////////////////////////////////////////////////
Task("Credentials")
.WithCriteria(() => isAppVeyor)
.Does(() =>
{
    Information(Figlet("Credentials"));
    
    gitHubToken = EnvironmentVariable("GITHUB_TOKEN");
    if (string.IsNullOrEmpty(gitHubToken))
    {
        throw new Exception("Environment variable 'GITHUB_TOKEN' is not set");
    }
    nugetReleaseToken = EnvironmentVariable("NUGET_TOKEN");
    if (string.IsNullOrEmpty(nugetReleaseToken))
    {
        throw new Exception("Environment variable 'NUGET_TOKEN' is not set");
    }
    appVeyorFeedToken = EnvironmentVariable("APPVEYOR_TOKEN");
    if (string.IsNullOrEmpty(appVeyorFeedToken))
    {
        throw new Exception("Environment variable 'APPVEYOR_TOKEN' is not set");
    }
    codeCovToken = EnvironmentVariable("CODECOV_TOKEN");
    if (string.IsNullOrEmpty(codeCovToken))
    {
        throw new Exception("Environment variable 'CODECOV_TOKEN' is not set");
    }
});

///////////////////////////////////////////////////////////////////////////////
// NuGet Feed
///////////////////////////////////////////////////////////////////////////////
Task("NuGetFeed")
.WithCriteria(() => isAppVeyor && BuildSystem.AppVeyor.Environment.Repository.Tag.IsTag)
.IsDependentOn("NuGet")
.IsDependentOn("Credentials")
.Does(() => 
{
    Information(Figlet("NuGet Feed"));
    
    var nugets = GetFiles($"{buildArtifacts.Path}/*.nupkg");
    foreach(var package in nugets)
    {
        if (package.GetFilename().ToString().Contains(".Sources"))
        {
            RunCommand("dotnet", $"nuget push \"{package.FullPath}\" --source \"{appVeyorNuGetFeed}\" --api-key \"{appVeyorFeedToken}\" --skip-duplicate");
        }
        else
        {
            RunCommand("dotnet", $"nuget push \"{package.FullPath}\" --source \"{nuGetSource}\" --api-key \"{nugetReleaseToken}\" --skip-duplicate");
        }
    }
    var symbols = GetFiles($"{buildArtifacts.Path}/*.snupkg");
    foreach(var symbol in symbols)
    {
        RunCommand("dotnet", $"nuget push \"{symbol.FullPath}\" --source \"{nuGetSource}\" --api-key \"{nugetReleaseToken}\" --skip-duplicate");
    }
});

///////////////////////////////////////////////////////////////////////////////
// Default
///////////////////////////////////////////////////////////////////////////////
Task("Default")
.IsDependentOn("Credentials")
.IsDependentOn("Version")
.IsDependentOn("Clean")
.IsDependentOn("Restore")
.IsDependentOn("Build")
.IsDependentOn("UnitTests")
.IsDependentOn("GenerateCoverage")
.IsDependentOn("UploadCoverage")
.IsDependentOn("AssertPackages")
.IsDependentOn("NuGet")
.IsDependentOn("NuGetFeed");

RunTarget(target);
