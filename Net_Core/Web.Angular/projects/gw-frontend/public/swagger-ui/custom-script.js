// Setup variables
let mTries = 0;
var mMaxTries = 10;
var mTimeout = 100;

/**
 * Checks if the document is ready and if the topbar wrapper exists.
 * If so, replace the current logo with the text "GrowthWare API".
 * If not, wait a bit and try again until the maximum number of tries has been reached.
 */
function checkReady() {
	if (document.readyState === 'complete') {
		var mTopbarWrapper = document.querySelectorAll('.topbar-wrapper');
		if (mTopbarWrapper) {
			// Remove the current logo and the current text
			mTopbarWrapper.forEach((element) => {
				element.querySelectorAll('svg').forEach((svg) => {
					svg.remove();
				});
			});
			// Add the new text (logo is being replaced in the custom-style.css file)
			mTopbarWrapper.forEach((element) => {
				var anchor = element.querySelector('a');
				var span = document.createElement('span');
				span.textContent = 'GrowthWare API';
				anchor.appendChild(span);
			});
		}
	} else {
		mTries++;
		if (mTries < mMaxTries) {
			setTimeout(checkReady, mTimeout);
		} else {
			console.log('Failed to load logo');
		}
	}
}

/**
 * Replaces the favicon in the header of the page with the GrowthWare favicon.
 * If a favicon does not already exist, create one.
 * @function
 */
function replaceFavIcon() {
// Replace the favicon
	var link = document.querySelector('link[rel~=\'icon\']');
	if (!link) {
		link = document.createElement('link');
		link.rel = 'icon';
		document.head.appendChild(link);
	}
	link.href = '/icons/favicon.ico';
}

// Call the functions
replaceFavIcon();
checkReady();
