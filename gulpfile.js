var gulp = require('gulp');
var ts = require('gulp-typescript');
var nodemon = require('gulp-nodemon');

var tsProject = ts.createProject({
    module: "commonjs"
});

gulp.task('typescript', function () {
    var tsResult = gulp.src('server/**/*.ts')
        .pipe(ts(tsProject));
    return tsResult.js.pipe(gulp.dest('build'));
});

gulp.task('watch', ['typescript'], function () {
    gulp.watch('server/**/*.ts', ['typescript']);
    nodemon({
        script: './build/server.js',
        ext: 'js',
        ignore: ['./build/public/*']
    })
        .on('restart', function () {
            console.log('restarted!')
        })
});