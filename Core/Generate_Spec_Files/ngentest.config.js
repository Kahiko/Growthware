const klassTemplate = require('./ejs-templates/class.template.js');
const componentTemplate = require('./ejs-templates/component.template.js');
const directiveTemplate = require('./ejs-templates/directive.template.js');
const injectableTemplate = require('./ejs-templates/injectable.template.js');
const pipeTemplate = require('./ejs-templates/pipe.template.js');

module.exports = {
  framework: 'karma', // or 'jest'
  outputTemplates: {
    klass: klassTemplate,  // ejs contents read from file
    component: componentTemplate,
    directive: directiveTemplate,
    injectable: injectableTemplate, 
    pipe: pipeTemplate 
  },
  // necessary directives used for a component test
  directives: [
    // 'myCustomDirective' // my custom directive used over application
  ], 
  // necessary pipes used for a component test
  pipes: [
    'translate', 'phoneNumber', 'safeHtml'
  ],
  // when convert to JS, some codes need to be replaced to work 
  replacements: [ // some 3rd party module causes an error
    { from: '^\\S+\\.define\\(.*\\);', to: ''} // some commands causes error
  ],
  // when constructor param type is as following, create a mock class with this properties
  // e.g. @Injectable() MockElementRef { nativeElement = {}; }
  providerMocks: {
    ElementRef: ['nativeElement = {};'],
    Router: ['navigate() {};'],
    Document: ['querySelector() {};'],
    HttpClient: ['post() {};'],
    TranslateService: ['translate() {};'],
    EncryptionService: [],
  }
}