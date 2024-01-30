const path = require('path')

const baseFolder =
    process.env.APPDATA !== undefined && process.env.APPDATA !== ''
        ? `${process.env.APPDATA}/ASP.NET/http`
        : `${process.env.HOME}/.aspnet/http`;

const certificateArg = process.argv.map(arg => arg.match(/--name=(?<value>.+)/i)).filter(Boolean)[0];
const certificateName = certificateArg ? certificateArg.groups.value : "vueapp";

if (!certificateName) {
    console.error('Invalid certificate name. Run this script in the context of an npm/yarn script or pass --name=<<app>> explicitly.')
    process.exit(-1);
}

const devTarget = "http://localhost:7214/";
module.exports = {
    devServer: {
        proxy: {
            '^/api/user/me': {
                target: devTarget
            },
            '^/api/user/signin': {
                target: devTarget
            },
            '^/api/user/token': {
                target: devTarget
            },
            '^/api/user/tokenrefresh': {
                target: devTarget
            },
            '^/api/likedmovie/get': {
                target: devTarget
            },
            '^/api/likedmovie/put': {
                target: devTarget
            },
            '^/api/likedmovie/delete': {
                target: devTarget
            },
            '^/api/likedperson/get': {
                target: devTarget
            },
            '^/api/likedperson/put': {
                target: devTarget
            },
            '^/api/likedperson/delete': {
                target: devTarget
            },
            '^/api/likedtv/get': {
                target: devTarget
            },
            '^/api/likedtv/put': {
                target: devTarget
            },
            '/api/imdb/ratings': {
                target: devTarget,
            },
            '^/api/likedtv/delete': {
                target: devTarget
            },
            '^/api/tmdb/get': {
                target: devTarget
            }
        },
        port: 5002
    }
}