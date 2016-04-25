var gulp = require('gulp');
var gutil = require('gulp-util');
var del = require('del');
var sq = require('gulp-sequence');
var rename = require('gulp-rename');
var usemin = require('gulp-usemin');
var uglify = require('gulp-uglify');
var minifyCss = require('gulp-minify-css');
var prefix = require('gulp-prefix');

gulp.task('default', function () {

});

gulp.task('clean-build', function (done) {
	gutil.log('Cleaning build folder...');
	del(['./build/**/*']).then(function () {
		done();
	});
});

gulp.task('build-dev', function () {
	gutil.log('Build(copy) HTML for DEV...');
	gulp.src('./index.template.html').pipe(rename('index.html')).pipe(gulp.dest('.'));
});

gulp.task('minify-files-prod', function () {
	gutil.log('Minifying files for DEPLOY...');
	return gulp.src('./index.template.html')
		.pipe(usemin({
			libjs: [uglify],
			libcss: [minifyCss, 'concat'],
			css: [minifyCss, 'concat'],
			js: [uglify]
		}))
		.pipe(gulp.dest('./build/'));
});

gulp.task('copy-html-back-prod', function () {

	var prefixPattern = "build";

	return gulp.src('./build/index.template.html').pipe(rename('index.html')).pipe(prefix(prefixPattern, [
		{ match: "script[src]", attr: "src" },
		{ match: "link[href]", attr: "href" }
	])).pipe(gulp.dest('.'));
});

gulp.task('clear-build-index', function(done) {
	del(['./build/index.template.html']).then(function () {
		done();
	});
});


gulp.task('build-prod', sq('clean-build', 'minify-files-prod', 'copy-html-back-prod', 'clear-build-index'));