import React from 'react';
import Router, {Route, RouteHandler, Link, DefaultRoute} from 'react-router';

import routes from '../common/components/Routes';

require('./styles/hello.css');
require('./styles/hello.less');

Router.run(routes, Router.HistoryLocation, (Root) => {
    React.render(<Root/>, document.getElementById('react-content'));
});

