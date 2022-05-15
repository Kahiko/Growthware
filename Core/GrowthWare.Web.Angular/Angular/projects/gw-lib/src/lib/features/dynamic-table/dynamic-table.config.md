The dynamic-table layout
┌─────────────────┬───────────────────────────────────────────────────────────────────────────┬─────────────────┐
│     button      │ RPP  Help                                                ST  Add          │     button      │ Second Row
├─────────────────┴───────────────────────────────────────────────────────────────────────────┴─────────────────┤
│                                           Heading                                                             │ First Row
│                                       Search Results                                                          │ Not counted as a row
├─────────────────┬───────────────────────────────────────────────────────────────────────────┬─────────────────┤
│     button      │                          Pager                                            │     button      │ Third Row
└─────────────────┴───────────────────────────────────────────────────────────────────────────┴─────────────────┘
RPP - Records Per-Page text box
ST  - Search Text               - id/name = 
Add - Add button
Information regarding the dynamic-table.config.json file and it's contents
"buttons" is an array of JSON object containing properties for the 5 buttons in the component.
    id      - string - use as the "id" of the HTML element, 
    name    - string - use as the "name" of the HTML element, 
    class   - string - used to set the "class" property of the HTML element, 
    text    - string - used to as the innerHTML of the HTML element, 
    visible - bool   - used to show/hide the button (when false the element will be given a style of "visibility: none")

The 5 button default values:
    id      - configurationName + button position + "Btn" ... searchAccountTopLeftBtn (index 0 or the "add" button is searchAccountAddBtn), 
    name    - configurationName + button position + "Btn" ... searchAccountTtopLeftBtn (index 0 or the "add" button is searchAccountAddBtn), 
    class   - "btn btn-primary", 
    text    - "button position" or "Add" in the case of the add button, 
    visible - false

  The index of the configuration is as follows:
    0 - The "Add" button
    1 - The top left button
    2 - The top right button
    3 - The bottom left button
    4 - The bottom right button
  The buttons are always set by the index in the array.  If you have less than 5 elements in the array,
  buttons the default style propery will be applied starting with the 5th element or index 4.
  Meaning if you have 3 elements in the array the botton left and right buttons will recieve the default sytle and
  therefore not be in the component.  More than 5 elements in the array will be ignored. 


