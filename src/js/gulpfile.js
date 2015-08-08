var gulp = require('gulp');
var ts = require('gulp-typescript');

gulp.task('default', function () {
    var tsResult = gulp.src(['server/**/*.ts', 'common/**/*.ts'])
        .pipe(ts({
            noImplicitAny: true,
            outDir : 'server-bin',
            module: 'commonjs',
            target: 'ES5'
        }));
    return tsResult.js.pipe(gulp.dest('server-bin'));
});
