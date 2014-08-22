module.exports = function(grunt) {
    grunt.loadNpmTasks('grunt-msbuild');
    grunt.loadNpmTasks('grunt-dotnet-assembly-info');
    grunt.loadNpmTasks('grunt-nunit-runner');
    grunt.loadNpmTasks('grunt-nuget');

    grunt.registerTask('default', ['msbuild', 'nunit']);
    grunt.registerTask('ci', ['assemblyinfo', 'msbuild', 'nunit', 'nugetpack']);
    grunt.registerTask('deploy', ['assemblyinfo', 'msbuild', 'nunit', 'nugetpack', 'nugetpush']);

    grunt.initConfig({
        assemblyinfo: {
            options: {
                files: ['src/Bender.sln'],
                info: {
                    version: process.env.BUILD_NUMBER, 
                    fileVersion: process.env.BUILD_NUMBER
                }
            }
        },
        msbuild: {
            src: ['src/Bender.sln'],
            options: {
                projectConfiguration: 'Release',
                targets: ['Clean', 'Rebuild'],
                version: 4.0,
                stdout: true
            }
        },
        nunit: {
            files: ['src/Bender.sln'],
            options: {
                teamcity: true
            }
        },
        nugetpack: {
            bender: {
                src: 'Bender.nuspec',
                dest: './'
            },
            options: {
                version: process.env.BUILD_NUMBER
            }
        },
        nugetpush: {
            bender: {
                src: '*.nupkg'
            },
            options: {
                apiKey: process.env.NUGET_API_KEY
            }
        }
    });
}