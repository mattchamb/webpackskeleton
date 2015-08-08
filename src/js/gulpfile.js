var gulp = require('gulp');
var del = require('del');
var ts = require('gulp-typescript');
var babel = require('gulp-babel');

gulp.task('clean', function (cb) {
    del(['build-temp', 'server-bin'], cb);
});

gulp.task('babel', function () {
    return gulp.src([
        'server/**/*.js',
        'common/**/*.js',
        'server/**/*.jsx',
        'common/**/*.jsx'
    ], {base: '.'})
        .pipe(babel())
        .pipe(gulp.dest('build-temp'))
        .pipe(gulp.dest('server-bin'));
});

gulp.task('asd', ['babel'], function () {
    return gulp.src(['server/**/*.ts', 'common/**/*.ts'], {base: '.'})
        .pipe(gulp.dest('build-temp'));
});

gulp.task('a', ['asd'], function () {
    var tsResult = gulp.src('build-temp/**/*.ts')
        .pipe(ts({
            noImplicitAny: true,
            module: 'commonjs',
            target: 'ES5'
        }));
    return tsResult.js.pipe(gulp.dest('server-bin'));
});

gulp.task('default', function () {

});
