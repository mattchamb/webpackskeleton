var gulp = require('gulp');
var ts = require('gulp-typescript');
var merge = require('merge2');

var tsProject = ts.createProject({
    declarationFiles: true,
    module: "commonjs"
});

gulp.task('scripts', function() {
    var tsResult = gulp.src('server/**/*.ts')
        .pipe(ts(tsProject));

    return merge([ // Merge the two output streams, so this task is finished when the IO of both operations are done. 
        tsResult.dts.pipe(gulp.dest('build/definitions')),
        tsResult.js.pipe(gulp.dest('build'))
    ]);
});

gulp.task('watch', ['scripts'], function() {
    gulp.watch('server/**/*.ts', ['scripts']);
});