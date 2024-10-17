DECLARE
   v_now DATE;
   /* */
   v_V_DeveloperID NUMBER(10,0);
   v_V_SE_SEQ_ID NUMBER(10,0);
   v_V_EVERYONE_ID NUMBER(10,0);
   v_V_MyAction VARCHAR2(256);
   v_V_FUNCTION_TYPE NUMBER(10,0);
   v_V_EnableViewStateTrue NUMBER(10,0);
   v_V_EnableViewStateFalse NUMBER(10,0);
   v_V_IsNavTrue NUMBER(10,0);
   v_V_IsNavFalse NUMBER(10,0);
   v_V_NAV_TYPE NUMBER(10,0);
   v_V_ParentID NUMBER(10,0);
   v_V_FunctionID NUMBER(10,0);
   v_V_ViewPermission NUMBER(10,0);
   v_V_AddPermission NUMBER(10,0);
   v_V_EditPermission NUMBER(10,0);
   v_V_DeletePermission NUMBER(10,0);
   v_V_NAV_TYPE_Hierarchical NUMBER(10,0);
   v_V_NAV_TYPE_Vertical NUMBER(10,0);
   v_V_NAV_TYPE_Horizontal NUMBER(10,0);
   v_V_CHANGE_PASSWORD NUMBER(10,0);
   v_V_INACTIVE NUMBER(10,0);
   v_V_ACTIVE NUMBER(10,0);
   v_V_ALLOW_HTML_INPUT NUMBER(10,0);
   v_V_ALLOW_COMMENT_HTML_INPUT NUMBER(10,0);
   v_V_IS_CONTENT NUMBER(10,0);
   v_V_FORMAT_AS_HTML_TRUE NUMBER(10,0);
   v_V_FORMAT_AS_HTML_FALSE NUMBER(10,0);
   v_V_PRIMARY_KEY NUMBER(10,0);
   v_V_ErrorCode NUMBER(10,0);
   v_V_FUNCTION_SEQ_ID NUMBER(10,0);
   v_V_ENCRYPTION_TYPE NUMBER(10,0);
   v_V_ENABLE_INHERITANCE NUMBER(10,0);
BEGIN
   v_now := SYSDATE;

   v_V_FORMAT_AS_HTML_TRUE := 1;-- FALSE
   

   v_V_FORMAT_AS_HTML_FALSE := 0;-- FALSE
   

   v_V_ALLOW_HTML_INPUT := 0;-- FALSE
   

   v_V_ALLOW_COMMENT_HTML_INPUT := 0;-- FALSE
   

   v_V_IS_CONTENT := 0;-- FALSE
   

   v_V_PRIMARY_KEY := NULL;-- Not needed when setup up the database
   

   v_V_ErrorCode := NULL;-- Not needed when setup up the database
   

   v_V_ENCRYPTION_TYPE := 1;-- TripleDES
   

   v_V_ENABLE_INHERITANCE := 1;-- 0 = FALSE 1 = TRUE
   

   -- Setup ZFC_SYSTEM_STATUS
   DBMS_OUTPUT.PUT_LINE('Adding System Status');

   ZFP_SET_SYSTEM_STATUS(-1,
                         'Active',
                         1,
                         v_now,
                         1,
                         v_now,
                         v_V_PRIMARY_KEY,
                         v_V_ErrorCode);

   ZFP_SET_SYSTEM_STATUS(-1,
                         'Inactive',
                         1,
                         v_now,
                         1,
                         v_now,
                         v_V_PRIMARY_KEY,
                         v_V_ErrorCode);

   ZFP_SET_SYSTEM_STATUS(-1,
                         'Disabled',
                         1,
                         v_now,
                         1,
                         v_now,
                         v_V_PRIMARY_KEY,
                         v_V_ErrorCode);

   ZFP_SET_SYSTEM_STATUS(-1,
                         'ChangePassword',
                         1,
                         v_now,
                         1,
                         v_now,
                         v_V_PRIMARY_KEY,
                         v_V_ErrorCode);

   SELECT STATUS_SEQ_ID
     INTO v_V_CHANGE_PASSWORD
     FROM ZFC_SYSTEM_STATUS
      WHERE DESCRIPTION = 'ChangePassword';

   SELECT STATUS_SEQ_ID
     INTO v_V_INACTIVE
     FROM ZFC_SYSTEM_STATUS
      WHERE DESCRIPTION = 'Inactive';

   SELECT STATUS_SEQ_ID
     INTO v_V_ACTIVE
     FROM ZFC_SYSTEM_STATUS
      WHERE DESCRIPTION = 'Active';

   --
   DBMS_OUTPUT.PUT_LINE('Adding Accounts');

   -- Add the anonymous account
   ZFP_SET_ACCOUNT(-1,
                       1,
                       'Anonymous',
                       'Anonymous',
                       'Anonymous',
                       '',
                       'Anonymous-Account',
                       'me@me.com',
                       'none',
                       v_now,
                       0,
                       1,
                       v_now,
                       v_now,
                       -5,
                       'none',
                       0,
                       0,
                       0,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

   -- BEFORE ADDING ANY MORE ACCOUNTS SETUP ZF_ACCT_CHOICES
   ZFP_SET_ACCT_CHOICES('Anonymous',
                            1,
                            'All',
                            '#ffffff',
                            '#eeeeee',
                            '#6699cc',
                            '#b6cbeb',
                            'Blue',
                            'FavoriateAction',
                            'ThinActions',
                            'WideActions',
                            5,
                            '');

   -- Add the system administrator account
   ZFP_SET_ACCOUNT(-1,
                       v_V_CHANGE_PASSWORD,
                       'Developer',
                       'System',
                       'Developer',
                       '',
                       'System-Developer',
                       'michael.regan@verizon.net',
                       'none',
                       v_now,
                       0,
                       1,
                       v_now,
                       v_now,
                       -5,
                       'none',
                       0,
                       1,
                       1,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

   -- testing account
   ZFP_SET_ACCOUNT(-1,
                       v_V_CHANGE_PASSWORD,
                       'Mike',
                       'System',
                       'Tester',
                       '',
                       'System-Tester',
                       'michael.regan@verizon.net',
                       'none',
                       v_now,
                       0,
                       0,
                       v_now,
                       v_now,
                       -5,
                       'none',
                       0,
                       0,
                       1,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

   SELECT ACCT_SEQ_ID
     INTO v_V_DeveloperID
     FROM ZFC_ACCTS
      WHERE ACCT = 'Developer';

   DBMS_OUTPUT.PUT_LINE('Adding DB Information');

   ZFP_SET_INFORMATION(-1,
                       '1.0',
                       v_V_ENABLE_INHERITANCE,
                       v_V_DeveloperID,
                       NULL,
                       NULL);

   DBMS_OUTPUT.PUT_LINE('Adding Function types');

   -- Setup ZFC_FUNCTION_TYPES
   ZFP_SET_FUNCTION_TYPES(-1,
                              'Module',
                              'used for modules',
                              '',
                              '0',
                              v_V_DeveloperID,
                              v_now,
                              v_V_DeveloperID,
                              v_now,
                              v_V_PRIMARY_KEY,
                              v_V_ErrorCode);

   ZFP_SET_FUNCTION_TYPES(-1,
                          'Security',
                          'used as a container for security.',
                          'none',
                          '0',
                          v_V_DeveloperID,
                          v_now,
                          v_V_DeveloperID,
                          v_now,
                          v_V_PRIMARY_KEY,
                          v_V_ErrorCode);

   ZFP_SET_FUNCTION_TYPES(-1,
                          'Menu Item',
                          'designates entry is a menu item.',
                          'none',
                          '0',
                          v_V_DeveloperID,
                          v_now,
                          v_V_DeveloperID,
                          v_now,
                          v_V_PRIMARY_KEY,
                          v_V_ErrorCode);

   ZFP_SET_FUNCTION_TYPES(-1,
                          'File Manager',
                          'Used for managing files and directories',
                          'Modules\System\FileManagement\FileManagerControl.ascx',
                          '0',
                          v_V_DeveloperID,
                          v_now,
                          v_V_DeveloperID,
                          v_now,
                          v_V_PRIMARY_KEY,
                          v_V_ErrorCode);

   ZFP_SET_FUNCTION_TYPES(-1,
                          'Articles',
                          'Contains articles that can represent news articles or other content',
                          'Modules\System\Content\ContentControl.ascx',
                          '1',
                          v_V_DeveloperID,
                          v_now,
                          v_V_DeveloperID,
                          v_now,
                          v_V_PRIMARY_KEY,
                          v_V_ErrorCode);

   ZFP_SET_FUNCTION_TYPES(-1,
                          'Links',
                          'Contains internal and external links',
                          'Modules\System\Content\ContentControl.ascx',
                          '1',
                          v_V_DeveloperID,
                          v_now,
                          v_V_DeveloperID,
                          v_now,
                          v_V_PRIMARY_KEY,
                          v_V_ErrorCode);

   ZFP_SET_FUNCTION_TYPES(-1,
                          'Downloads',
                          'Allows downloading of files',
                          'Modules\System\Content\ContentControl.ascx',
                          '1',
                          v_V_DeveloperID,
                          v_now,
                          v_V_DeveloperID,
                          v_now,
                          v_V_PRIMARY_KEY,
                          v_V_ErrorCode);

   ZFP_SET_FUNCTION_TYPES(-1,
                          'Photo Gallery',
                          'Enables you to display a gallery of images',
                          'Modules\System\Content\ContentControl.ascx',
                          '1',
                          v_V_DeveloperID,
                          v_now,
                          v_V_DeveloperID,
                          v_now,
                          v_V_PRIMARY_KEY,
                          v_V_ErrorCode);

   ZFP_SET_FUNCTION_TYPES(-1,
                          'Books',
                          'Contains book listings',
                          'Modules\System\Content\ContentControl.ascx',
                          '1',
                          v_V_DeveloperID,
                          v_now,
                          v_V_DeveloperID,
                          v_now,
                          v_V_PRIMARY_KEY,
                          v_V_ErrorCode);

   ZFP_SET_FUNCTION_TYPES(-1,
                          'Events',
                          'Contains event listings',
                          'Modules\System\Content\ContentControl.ascx',
                          '1',
                          v_V_DeveloperID,
                          v_now,
                          v_V_DeveloperID,
                          v_now,
                          v_V_PRIMARY_KEY,
                          v_V_ErrorCode);

   ZFP_SET_FUNCTION_TYPES(-1,
                          'HTML Page',
                          'Contains a single editable HTML page',
                          'Modules\System\Content\ContentControl.ascx',
                          '1',
                          v_V_DeveloperID,
                          v_now,
                          v_V_DeveloperID,
                          v_now,
                          v_V_PRIMARY_KEY,
                          v_V_ErrorCode);

   ZFP_SET_FUNCTION_TYPES(-1,
                          'Discuss',
                          'Contains a discussion area in which users can add posts',
                          'Modules\System\Content\ContentControl.ascx',
                          '1',
                          v_V_DeveloperID,
                          v_now,
                          v_V_DeveloperID,
                          v_now,
                          v_V_PRIMARY_KEY,
                          v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding navigation types');

   -- Setup ZFC_NAVIGATION_TYPES
   ZFP_SET_NAVIGATION_TYPES(-1,
                                'Horizontal',
                                v_V_DeveloperID,
                                v_now,
                                v_V_DeveloperID,
                                v_now,
                                v_V_PRIMARY_KEY,
                                v_V_ErrorCode);

   ZFP_SET_NAVIGATION_TYPES(-1,
                            'Vertical',
                            v_V_DeveloperID,
                            v_now,
                            v_V_DeveloperID,
                            v_now,
                            v_V_PRIMARY_KEY,
                            v_V_ErrorCode);

   ZFP_SET_NAVIGATION_TYPES(-1,
                            'Hierarchical',
                            v_V_DeveloperID,
                            v_now,
                            v_V_DeveloperID,
                            v_now,
                            v_V_PRIMARY_KEY,
                            v_V_ErrorCode);

   SELECT NAV_TYPE_SEQ_ID
     INTO v_V_NAV_TYPE_Hierarchical
     FROM ZFC_NAVIGATION_TYPES
      WHERE DESCRIPTION = 'Hierarchical';

   SELECT NAV_TYPE_SEQ_ID
     INTO v_V_NAV_TYPE_Vertical
     FROM ZFC_NAVIGATION_TYPES
      WHERE DESCRIPTION = 'Vertical';

   SELECT NAV_TYPE_SEQ_ID
     INTO v_V_NAV_TYPE_Horizontal
     FROM ZFC_NAVIGATION_TYPES
      WHERE DESCRIPTION = 'Horizontal';

   DBMS_OUTPUT.PUT_LINE('Adding permissions');

   -- Setup ZFC_PERMISSIONS
   ZFP_SET_PERMISSIONS(-1,
                           'View',
                           v_V_DeveloperID,
                           v_now,
                           v_V_DeveloperID,
                           v_now,
                           v_V_PRIMARY_KEY,
                           v_V_ErrorCode);

   ZFP_SET_PERMISSIONS(-1,
                       'Edit',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

   ZFP_SET_PERMISSIONS(-1,
                       'Add',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

   ZFP_SET_PERMISSIONS(-1,
                       'Delete',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

   SELECT PERMISSIONS_ID
     INTO v_V_ViewPermission
     FROM ZFC_PERMISSIONS
      WHERE DESCRIPTION = 'View';

   SELECT PERMISSIONS_ID
     INTO v_V_AddPermission
     FROM ZFC_PERMISSIONS
      WHERE DESCRIPTION = 'Add';

   SELECT PERMISSIONS_ID
     INTO v_V_EditPermission
     FROM ZFC_PERMISSIONS
      WHERE DESCRIPTION = 'Edit';

   SELECT PERMISSIONS_ID
     INTO v_V_DeletePermission
     FROM ZFC_PERMISSIONS
      WHERE DESCRIPTION = 'Delete';

   DBMS_OUTPUT.PUT_LINE('Adding Security Entity');

   ZFP_SET_SECURITY_ENTITIES(-1,
                          'All',
                          'All Security Entitys',
                          'no url',
                          1,
                          'SQLServer',
                          'FoundationFramework',
                          'Foundation.Framework.SQLServer',
                          'Server=localhost;Initial Catalog=Foundation;Integrated Security=true;',
                          'Blue Arrow',
                          'Default',
                          v_V_ENCRYPTION_TYPE,
                          -1,
                          v_V_PRIMARY_KEY,
                          v_V_ErrorCode);

   SELECT SE_SEQ_ID
     INTO v_V_SE_SEQ_ID
     FROM ZFC_SECURITY_ENTITIES
      WHERE NAME = 'All';

   DBMS_OUTPUT.PUT_LINE('Adding roles');

   -- Setup ZF_RLS
   ZFP_SET_ROLE(-1,
                    'Anonymous',
                    'The anonymous role.',
                    1,
                    0,
                    1,
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   ZFP_SET_ROLE(-1,
                'Authenticated',
                'The authenticated role.',
                1,
                1,
                1,
                v_V_DeveloperID,
                v_now,
                v_V_DeveloperID,
                v_now,
                v_V_PRIMARY_KEY,
                v_V_ErrorCode);

   ZFP_SET_ROLE(-1,
                'Developer',
                'The developer role.',
                1,
                1,
                1,
                v_V_DeveloperID,
                v_now,
                v_V_DeveloperID,
                v_now,
                v_V_PRIMARY_KEY,
                v_V_ErrorCode);

   ZFP_SET_ROLE(-1,
                'AlwaysLogon',
                'Assign this role to allow logon when the system is under maintance.',
                1,
                0,
                1,
                v_V_DeveloperID,
                v_now,
                v_V_DeveloperID,
                v_now,
                v_V_PRIMARY_KEY,
                v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding Groups');

   -- group id,group name,group description,Security Entity,added by,added date,updated by,updated date
   ZFP_SET_GROUP(-1,
                     'Everyone',
                     'Group representing both the authenticated and the anonymous roles.',
                     1,
                     v_V_DeveloperID,
                     v_now,
                     v_V_DeveloperID,
                     v_now,
                     v_V_PRIMARY_KEY,
                     v_V_ErrorCode);

   SELECT GROUP_SEQ_ID
     INTO v_V_EVERYONE_ID
     FROM ZFC_SECURITY_GRPS
      WHERE NAME = 'Everyone';

   -- group id, Security Entity,comma sep roles,added by,ErrorCode
   ZFP_SET_GROUP_RLS(v_V_EVERYONE_ID,
                         v_V_SE_SEQ_ID,
                         'Authenticated,Anonymous',
                         v_V_DeveloperID,
                         NULL);

   DBMS_OUTPUT.PUT_LINE('Adding account security');

   -- Setup the security
   -- Setup the account security
   ZFP_SET_ACCT_RLS('Anonymous',
                        1,
                        'Anonymous',
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   ZFP_SET_ACCT_RLS('Developer',
                    1,
                    'Developer,Authenticated,AlwaysLogon',
                    v_V_DeveloperID,
                    v_V_ErrorCode);

   ZFP_SET_ACCT_RLS('mike',
                    1,
                    'Authenticated',
                    v_V_DeveloperID,
                    v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding functions');

   -- Add functions
   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_EnableViewStateTrue := 1;

   v_V_EnableViewStateFalse := 0;

   v_V_IsNavTrue := 1;

   v_V_IsNavFalse := 0;

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Menu Item';

   v_V_MyAction := 'mnuRootMenu';

   ZFP_SET_FUNCTION(-1,
                    'Root Menu',
                    'Place Holer',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'none',
                    v_V_EnableViewStateFalse,
                    v_V_IsNavFalse,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    0,
                    'mnuRootHolder',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   v_V_MyAction := 'GenericHome';

   ZFP_SET_FUNCTION(-1,
                    'Home',
                    'Generic Home',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Home\GenericHome.ascx',
                    v_V_EnableViewStateFalse,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Horizontal,
                    'GenericHome',
                    1,
                    'Shown when not authenticated',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Anonymous',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   v_V_MyAction := 'Home';

   ZFP_SET_FUNCTION(-1,
                    'Home',
                    'Home',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Home\Home.ascx',
                    v_V_EnableViewStateFalse,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Horizontal,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Shown when authenticated',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Authenticated',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   v_V_MyAction := 'Logon';

   ZFP_SET_FUNCTION(-1,
                    'Logon',
                    'Logon',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Accounts\Logon.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Vertical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Loggs on an account',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Anonymous,Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   v_V_MyAction := 'Logoff';

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Menu Item';

   ZFP_SET_FUNCTION(-1,
                    'Logoff',
                    'Logoff',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Accounts\Logoff.ascx',
                    v_V_EnableViewStateFalse,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Horizontal,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Loggs off the system.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Authenticated',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Menu Item';

   v_V_MyAction := 'mnuAdmin';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuRootMenu';

   ZFP_SET_FUNCTION(-1,
                    'Admin',
                    'Administration',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'none',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Administration tasks.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Menu Item';

   v_V_MyAction := 'mnuCalendars';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuRootMenu';

   ZFP_SET_FUNCTION(-1,
                    'Calendars',
                    'Calendars',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'none',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Access to the calendar.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Authenticated,Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Menu Item';

   v_V_MyAction := 'mnuReports';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuRootMenu';

   ZFP_SET_FUNCTION(-1,
                    'Reports',
                    'Reports',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'none',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Access to the reports.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Menu Item';

   v_V_MyAction := 'mnuMyProfile';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuRootMenu';

   ZFP_SET_FUNCTION(-1,
                    'My Profile',
                    'My Profile',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'none',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Access to profile information.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Authenticated,Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding System Administrator menu');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Menu Item';

   v_V_MyAction := 'mnuSystem Administration';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuRootMenu';

   ZFP_SET_FUNCTION(-1,
                    'SysAdmin',
                    'System Administration',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'none',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Serves as the root menu item for the hierarchical menus.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuRootMenu';

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding Manage Functions');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Menu Item';

   v_V_MyAction := 'mnuManage Functions';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuSystem Administration';

   ZFP_SET_FUNCTION(-1,
                    'Manage Functions',
                    'Manage Functions',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'none',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Menu item for functions.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding Add Functions');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'AddFunctions';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuManage Functions';

   ZFP_SET_FUNCTION(-1,
                    'Add Functions',
                    'Add Functions',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Administration\Functions\AddEditFunctions.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Adds a function to the system.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding Copy Function Security');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'Copy Function Security';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuManage Functions';

   ZFP_SET_FUNCTION(-1,
                    'Copy Function Security',
                    'Copy Function Security',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Administration\Functions\CopyFunctionSecurity.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Adds a function to the system.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding Manage Security Entitys');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Menu Item';

   v_V_MyAction := 'mnuManage Security Entitys';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuSystem Administration';

   ZFP_SET_FUNCTION(-1,
                    'Manage Security Entitys',
                    'Manage Security Entitys',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'none',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Menu item for Security Entitys.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('File Management menu');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Menu Item';

   v_V_MyAction := 'mnuManageFiles';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuSystem Administration';

   ZFP_SET_FUNCTION(-1,
                    'Manage Files',
                    'Manage Files',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'none',
                    v_V_EnableViewStateFalse,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Used to manage files.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('cache directory management');

   -- Add module
   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'File Manager';

   v_V_MyAction := 'Manage Cachedependency';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuManageFiles';

   ZFP_SET_FUNCTION(-1,
                    'Manage Cachedependency',
                    'Manage Cachedependency',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\FileManagement\FileManagerControl.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Used to manage the cache dependency direcory.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   -- Set security
   SELECT function_seq_id
     INTO v_V_FUNCTION_SEQ_ID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FUNCTION_SEQ_ID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   ZFP_SET_FUNCTION_RLS(v_V_FUNCTION_SEQ_ID,
                        1,
                        'Developer',
                        v_V_AddPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   ZFP_SET_FUNCTION_RLS(v_V_FUNCTION_SEQ_ID,
                        1,
                        'Developer',
                        v_V_EditPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   ZFP_SET_FUNCTION_RLS(v_V_FUNCTION_SEQ_ID,
                        1,
                        'Developer',
                        v_V_DeletePermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding cache directory');

   -- Add directory information
   SELECT FUNCTION_SEQ_ID
     INTO v_V_FUNCTION_SEQ_ID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_DIRECTORY(v_V_FUNCTION_SEQ_ID,
                     'D:\WebProjects\2005\Foundation\FoundationProjects\FoundationWeb\CacheDependency',
                     0,
                     '',
                     '',
                     v_V_DeveloperID,
                     v_now,
                     v_V_DeveloperID,
                     v_now,
                     v_V_PRIMARY_KEY,
                     v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('cache directory management');

   -- Add module
   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'File Manager';

   v_V_MyAction := 'Manage Logs';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuManageFiles';

   ZFP_SET_FUNCTION(-1,
                    'Manage Logs',
                    'Manage Logs',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\FileManagement\FileManagerControl.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Used to manage the logs direcory.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FUNCTION_SEQ_ID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   -- Set security
   ZFP_SET_FUNCTION_RLS(v_V_FUNCTION_SEQ_ID,
                             1,
                             'Developer',
                             v_V_ViewPermission,
                             v_V_DeveloperID,
                             v_V_ErrorCode);

   ZFP_SET_FUNCTION_RLS(v_V_FUNCTION_SEQ_ID,
                        1,
                        'Developer',
                        v_V_AddPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   ZFP_SET_FUNCTION_RLS(v_V_FUNCTION_SEQ_ID,
                        1,
                        'Developer',
                        v_V_EditPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   ZFP_SET_FUNCTION_RLS(v_V_FUNCTION_SEQ_ID,
                        1,
                        'Developer',
                        v_V_DeletePermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding log log');

   -- Add directory information
   SELECT FUNCTION_SEQ_ID
     INTO v_V_FUNCTION_SEQ_ID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_DIRECTORY(v_V_FUNCTION_SEQ_ID,
                     'D:\WebProjects\2005\Foundation\FoundationProjects\FoundationWeb\Logs',
                     0,
                     '',
                     '',
                     v_V_DeveloperID,
                     v_now,
                     v_V_DeveloperID,
                     v_now,
                     v_V_PRIMARY_KEY,
                     v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding Manage Name/Value Pairs');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Menu Item';

   v_V_MyAction := 'mnuManage Name Value Pairs';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuSystem Administration';

   ZFP_SET_FUNCTION(-1,
                    'Manage Name/Value Pairs',
                    'Manage Name/Value Pairs',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'none',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Menu item for name/value pairs.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding Add Edit Groups');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'Add Edit Groups';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuAdmin';

   ZFP_SET_FUNCTION(-1,
                    'Add Edit Groups',
                    'Add Edit Groups',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Administration\Groups\AddEditGroups.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Used to add or edit roles.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_AddPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_EditPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_DeletePermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding Manage Messages');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Menu Item';

   v_V_MyAction := 'mnuManage Messages';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuAdmin';

   ZFP_SET_FUNCTION(-1,
                    'Manage Messages',
                    'Manage Messages',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'none',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Menu item for messages.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding Manage States');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Menu Item';

   v_V_MyAction := 'mnuManage States';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuSystem Administration';

   ZFP_SET_FUNCTION(-1,
                    'Manage States',
                    'Manage States',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'none',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Menu item for states.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding Manage Work Flows');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Menu Item';

   v_V_MyAction := 'mnuWork Flows';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuSystem Administration';

   ZFP_SET_FUNCTION(-1,
                    'Manage Work Flows',
                    'Manage Work Flows',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'none',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Menu item for work flows.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding Encryption Helper');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'EncryptionHelper';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuSystem Administration';

   ZFP_SET_FUNCTION(-1,
                    'Encryption Helper',
                    'Encryption Helper',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Administration\Encrypt\EncryptDecrypt.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Menu item for work flows.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding GUID Helper');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'GUIDHelper';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuSystem Administration';

   ZFP_SET_FUNCTION(-1,
                    'GUID Helper',
                    'Displays''s a GUID',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Administration\Encrypt\GUIDHelper.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Displays a GUID may be necessary if you need to change the GUID in your project files.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding Random Number');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'Random Numbers';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuSystem Administration';

   ZFP_SET_FUNCTION(-1,
                    'Random Numbers',
                    'Displays''s a set of randomly generated number''s',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Administration\Encrypt\RandomNumbers.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Displays''s a set of randomly generated number''s.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding Set Log Level');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'SetLogLevel';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuSystem Administration';

   ZFP_SET_FUNCTION(-1,
                    'Set Log Level',
                    'Set Log Level',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Administration\Logs\SetLogLevel.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Used to set the log level of the application ... Debug, Error, Warn, Fatal.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding Update Anonymous Profile');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'UpdateAnonymousProfile';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuSystem Administration';

   ZFP_SET_FUNCTION(-1,
                    'Update Anonymous Profile',
                    'Update Anonymous Profile',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Administration\AnonymousAccount\UpdateAnonymousCache.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Remove any cached information for the anonymous account.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding Search Functions');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'SearchFunctions';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuManage Functions';

   ZFP_SET_FUNCTION(-1,
                    'Search Functions',
                    'Search Functions',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Administration\Functions\SearchFunctions.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Searches for functions in the system.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_DeletePermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding Edit Functions');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'EditFunctions';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuSystem Administration';

   ZFP_SET_FUNCTION(-1,
                    'Edit Functions',
                    'Edit Functions',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Administration\Functions\AddEditFunctions.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavFalse,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Edits a function in the system.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding Function Security');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'Function Security';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuReports';

   ZFP_SET_FUNCTION(-1,
                    'Function Security',
                    'Function Security',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Reports\FunctionSecurity.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Displays a report for function security.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding Security By Role');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'Security By Role';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuReports';

   ZFP_SET_FUNCTION(-1,
                    'Security By Role',
                    'Security By Role',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Reports\SecurityByRole.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Displays a report for security by role.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding Change Password');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'Change Password';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuMyProfile';

   ZFP_SET_FUNCTION(-1,
                    'Change Password',
                    'Change Password',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Accounts\ChangePassword.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Used to change an accounts password.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Authenticated',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding Change Colors');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'Change Colors';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuMyProfile';

   ZFP_SET_FUNCTION(-1,
                    'Change Colors',
                    'Change Colors',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Accounts\ChangeColors.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Used to change an accounts color scheme.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Authenticated',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding Select Preferences');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'Select Preferences';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuMyProfile';

   ZFP_SET_FUNCTION(-1,
                    'Select Preferences',
                    'Select Preferences',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Accounts\SelectPreferences.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Used to select preference for an account, records per page etc.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Authenticated',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding Edit Account');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'Edit Account';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuMyProfile';

   ZFP_SET_FUNCTION(-1,
                    'Edit Account',
                    'Edit Account',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Administration\Accounts\AddEditAccount.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Used to edit an account profile.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Authenticated',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding Edit Other Account');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'Edit Other Account';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuMyProfile';

   ZFP_SET_FUNCTION(-1,
                    'Edit Other Account',
                    'Edit Other Account',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Administration\Accounts\AddEditAccount.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavFalse,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Used to edit anothers account profile.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding Community Calendar');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'Community Calendar';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuCalendars';

   ZFP_SET_FUNCTION(-1,
                    'Community Calendar',
                    'Community Calendar',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Calendar\CommunityCalendar.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Used to show calendar data.  Created as an example module.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Authenticated',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding Add Account');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'Add Account';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuAdmin';

   ZFP_SET_FUNCTION(-1,
                    'Add Account',
                    'Add Account',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Administration\Accounts\AddEditAccount.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Used to add an accounts password.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding Add Edit Roles');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'Add Edit Roles';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuAdmin';

   ZFP_SET_FUNCTION(-1,
                    'Add Edit Roles',
                    'Add Edit Roles',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Administration\Roles\AddEditRoles.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Used to add or edit roles.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_AddPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_EditPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_DeletePermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding Add Edit Name Value Pairs Details');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'Add Edit List of values';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuAdmin';

   ZFP_SET_FUNCTION(-1,
                    'Add Edit List of Values',
                    'Add Edit List of Values',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Administration\LOV\AddEditLOV.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Used to add or edit a list of value details.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding ViewAccountRoleTab');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Security';

   v_V_MyAction := 'ViewAccountRoleTab';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuAdmin';

   ZFP_SET_FUNCTION(-1,
                    'ViewAccountRoleTab',
                    'View Accounts Roles Tab',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'None',
                    v_V_EnableViewStateFalse,
                    v_V_IsNavFalse,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Used as a security holder for roles that can view the accounts role tab.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding ViewFunctionRoleTab');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Security';

   v_V_MyAction := 'ViewFunctionRoleTab';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuAdmin';

   ZFP_SET_FUNCTION(-1,
                    'ViewFunctionRoleTab',
                    'View Functions Roles Tab',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'None',
                    v_V_EnableViewStateFalse,
                    v_V_IsNavFalse,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Used as a security holder for roles that can view the functions role tab.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding ViewAccountGroupTab');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Security';

   v_V_MyAction := 'ViewAccountGroupTab';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuAdmin';

   ZFP_SET_FUNCTION(-1,
                    'ViewAccountGroupTab',
                    'View Accounts Groups Tab',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'None',
                    v_V_EnableViewStateFalse,
                    v_V_IsNavFalse,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Used as a security holder for groups that can view the accounts group tab.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding ViewFunctionGroupTab');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Security';

   v_V_MyAction := 'ViewFunctionGroupTab';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuAdmin';

   ZFP_SET_FUNCTION(-1,
                    'ViewFunctionGroupTab',
                    'View Function Groups Tab',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'None',
                    v_V_EnableViewStateFalse,
                    v_V_IsNavFalse,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Used as a security holder for groups that can view the functions group tab.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding Search Accounts');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'Search Accounts';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuAdmin';

   ZFP_SET_FUNCTION(-1,
                    'Search Accounts',
                    'Search Accounts',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Administration\Accounts\SearchAccounts.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Used to select accounts for edit.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding Edit Role Members');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'Edit Role Members';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuAdmin';

   ZFP_SET_FUNCTION(-1,
                    'Edit Role Members',
                    'Edit Role Members',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Administration\Roles\EditRoleMembers.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavFalse,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Used to add or remove members of a role.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_EditPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding Edit Group Members');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'Edit Group Members';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuAdmin';

   ZFP_SET_FUNCTION(-1,
                    'Edit Group Members',
                    'Edit Group Members',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Administration\Groups\EditGroupMembers.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavFalse,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Used to add or remove members of a role.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding Navigation');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'VerticalMenu';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuAdmin';

   ZFP_SET_FUNCTION(-1,
                    'Navigation',
                    'Navigation',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Navigation\VerticalMenuUserControl.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavFalse,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Contains link items for the vertical menus.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Anonymous,Authenticated',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding Not Avalible');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'NotAvalible';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuAdmin';

   ZFP_SET_FUNCTION(-1,
                    'Not Avalible',
                    'Not Avalible',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Errors\NotAvailable.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavFalse,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Error page when the action is not avalible.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Anonymous,Authenticated',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   --AccessDenied
   DBMS_OUTPUT.PUT_LINE('Adding Access Denied');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'AccessDenied';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuAdmin';

   ZFP_SET_FUNCTION(-1,
                    'Access Denied',
                    'Access Denied',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Errors\AccessDenied.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavFalse,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Error page when the account being used does not have sufficient access to the view permission.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Anonymous,Authenticated',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   --Adding Error
   DBMS_OUTPUT.PUT_LINE('Adding Error');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'DisplayError';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuAdmin';

   ZFP_SET_FUNCTION(-1,
                    'Display Error',
                    'Display Error',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Errors\DisplayError.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavFalse,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Error page when unknown or unexpected error occurs.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Anonymous,Authenticated',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   --Select A Security Entity
   DBMS_OUTPUT.PUT_LINE('Adding Select A Security Entity');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'Select A Security Entity';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuAdmin';

   ZFP_SET_FUNCTION(-1,
                    'Select A Security Entity',
                    'Select A Security Entity',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\SecurityEntities\SelectSecurityEntity.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Vertical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Used to select a Security Entity.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Authenticated',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   -- Web configuration
   DBMS_OUTPUT.PUT_LINE('Adding Web configuration');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'Web Config';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuSystem Administration';

   ZFP_SET_FUNCTION(-1,
                    'Web Config',
                    'Web Config',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Administration\Configuration\AddEditWebConfig.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Adds or edits web.config file settings.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   -- Line Count
   DBMS_OUTPUT.PUT_LINE('Adding Line Count');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'Line Count';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuSystem Administration';

   ZFP_SET_FUNCTION(-1,
                    'Line Count',
                    'Line Count',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\LineCount.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Utility to count the lines of code.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   -- Add a Security Entity
   DBMS_OUTPUT.PUT_LINE('Adding Add Security Entitys');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'Add Security Entitys';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuManage Security Entitys';

   ZFP_SET_FUNCTION(-1,
                    'Add Security Entitys',
                    'Add Security Entitys',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Administration\SecurityEntities\AddEditSecurityEntities.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Used to add a Security Entity.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   -- Edit a Security Entity
   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'Edit a Security Entity';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuManage Security Entitys';

   ZFP_SET_FUNCTION(-1,
                    'Edit a Security Entity',
                    'Edit a Security Entity',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Administration\SecurityEntities\AddEditSecurityEntities.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavFalse,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Used to edit a Security Entity.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   -- Search Security Entitys
   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'Search Security Entitys';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuManage Security Entitys';

   ZFP_SET_FUNCTION(-1,
                    'Search Security Entitys',
                    'Search Security Entitys',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Administration\SecurityEntities\SearchSecurityEntities.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Used to search a Security Entity.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   -- Add a Name Value Pair
   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'Add Name Value Pairs';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuManage Name Value Pairs';

   ZFP_SET_FUNCTION(-1,
                    'Add Name Value Pairs',
                    'Add Security Entitys',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Administration\NVP\AddEditNVP.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Used to add a name/value pair.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   -- Edit a Name Value Pair
   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'Edit Name Value Pairs';

   --SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuManage Security Entitys')
   ZFP_SET_FUNCTION(-1,
                         'Edit a Name Value Pair',
                         'Edit a Name Value Pair',
                         v_V_FUNCTION_TYPE,
                         v_V_ALLOW_HTML_INPUT,
                         v_V_ALLOW_COMMENT_HTML_INPUT,
                         'Modules\System\Administration\NVP\AddEditNVP.ascx',
                         v_V_EnableViewStateTrue,
                         v_V_IsNavFalse,
                         v_V_NAV_TYPE_Hierarchical,
                         v_V_MyAction,
                         v_V_ParentID,
                         'Used to edit a name/value pair.',
                         v_V_DeveloperID,
                         v_now,
                         v_V_DeveloperID,
                         v_now,
                         v_V_PRIMARY_KEY,
                         v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   -- Search Name Value Pairs
   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'Search Name Value Pairs';

   --SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuManage Security Entitys')
   ZFP_SET_FUNCTION(-1,
                         'Search Name Value Pairs',
                         'Search Name Value Pairs',
                         v_V_FUNCTION_TYPE,
                         v_V_ALLOW_HTML_INPUT,
                         v_V_ALLOW_COMMENT_HTML_INPUT,
                         'Modules\System\Administration\NVP\SearchNVP.ascx',
                         v_V_EnableViewStateTrue,
                         v_V_IsNavTrue,
                         v_V_NAV_TYPE_Hierarchical,
                         v_V_MyAction,
                         v_V_ParentID,
                         'Used to search a name/value pair.',
                         v_V_DeveloperID,
                         v_now,
                         v_V_DeveloperID,
                         v_now,
                         v_V_PRIMARY_KEY,
                         v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   -- Add a Message
   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'Add Message';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuManage Messages';

   ZFP_SET_FUNCTION(-1,
                    'Add Message',
                    'Add Message',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Administration\Messages\AddEditMessage.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Used to add a message.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   -- Edit a Message
   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'Edit Message';

   --SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuManage Security Entitys')
   ZFP_SET_FUNCTION(-1,
                         'Edit a Message',
                         'Edit a Message',
                         v_V_FUNCTION_TYPE,
                         v_V_ALLOW_HTML_INPUT,
                         v_V_ALLOW_COMMENT_HTML_INPUT,
                         'Modules\System\Administration\Messages\AddEditMessage.ascx',
                         v_V_EnableViewStateTrue,
                         v_V_IsNavFalse,
                         v_V_NAV_TYPE_Hierarchical,
                         v_V_MyAction,
                         v_V_ParentID,
                         'Used to edit a Message.',
                         v_V_DeveloperID,
                         v_now,
                         v_V_DeveloperID,
                         v_now,
                         v_V_PRIMARY_KEY,
                         v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   -- Search Message
   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'Search Messages';

   --SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuManage Security Entitys')
   ZFP_SET_FUNCTION(-1,
                         'Search Messages',
                         'Search Messages',
                         v_V_FUNCTION_TYPE,
                         v_V_ALLOW_HTML_INPUT,
                         v_V_ALLOW_COMMENT_HTML_INPUT,
                         'Modules\System\Administration\Messages\SearchMessages.ascx',
                         v_V_EnableViewStateTrue,
                         v_V_IsNavTrue,
                         v_V_NAV_TYPE_Hierarchical,
                         v_V_MyAction,
                         v_V_ParentID,
                         'Used to search a Message.',
                         v_V_DeveloperID,
                         v_now,
                         v_V_DeveloperID,
                         v_now,
                         v_V_PRIMARY_KEY,
                         v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   -- Edit a State
   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'Edit State';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuManage States';

   ZFP_SET_FUNCTION(-1,
                    'Edit a State',
                    'Edit a State',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Administration\States\AddEditStates.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavFalse,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Used to edit a State.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   -- Search State
   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'Search States';

   --SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuManage Security Entitys')
   ZFP_SET_FUNCTION(-1,
                         'Search States',
                         'Search States',
                         v_V_FUNCTION_TYPE,
                         v_V_ALLOW_HTML_INPUT,
                         v_V_ALLOW_COMMENT_HTML_INPUT,
                         'Modules\System\Administration\States\SearchStates.ascx',
                         v_V_EnableViewStateTrue,
                         v_V_IsNavTrue,
                         v_V_NAV_TYPE_Hierarchical,
                         v_V_MyAction,
                         v_V_ParentID,
                         'Used to search a State.',
                         v_V_DeveloperID,
                         v_now,
                         v_V_DeveloperID,
                         v_now,
                         v_V_PRIMARY_KEY,
                         v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   -- Add a Workflows
   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'Add a Workflow';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuWork Flows';

   ZFP_SET_FUNCTION(-1,
                    'Add a Workflow',
                    'Add a Workflow',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Administration\Messages\AddEditMessage.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Used to add a Workflow.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   -- Edit a Workflows
   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'Edit a Workflow';

   --SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuManage Security Entitys')
   ZFP_SET_FUNCTION(-1,
                         'Edit a Workflow',
                         'Edit a Workflow',
                         v_V_FUNCTION_TYPE,
                         v_V_ALLOW_HTML_INPUT,
                         v_V_ALLOW_COMMENT_HTML_INPUT,
                         'Modules\System\Administration\Messages\AddEditMessage.ascx',
                         v_V_EnableViewStateTrue,
                         v_V_IsNavFalse,
                         v_V_NAV_TYPE_Hierarchical,
                         v_V_MyAction,
                         v_V_ParentID,
                         'Used to edit a Workflow.',
                         v_V_DeveloperID,
                         v_now,
                         v_V_DeveloperID,
                         v_now,
                         v_V_PRIMARY_KEY,
                         v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   -- Search Workflows
   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'Search Workflows';

   --SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuManage Security Entitys')
   ZFP_SET_FUNCTION(-1,
                         'Search Workflows',
                         'Search Workflows',
                         v_V_FUNCTION_TYPE,
                         v_V_ALLOW_HTML_INPUT,
                         v_V_ALLOW_COMMENT_HTML_INPUT,
                         'Modules\System\Administration\Messages\SearchMessages.ascx',
                         v_V_EnableViewStateTrue,
                         v_V_IsNavTrue,
                         v_V_NAV_TYPE_Hierarchical,
                         v_V_MyAction,
                         v_V_ParentID,
                         'Used to search a Workflows.',
                         v_V_DeveloperID,
                         v_now,
                         v_V_DeveloperID,
                         v_now,
                         v_V_PRIMARY_KEY,
                         v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   -- Update Session
   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'Update';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuSystem Administration';

   ZFP_SET_FUNCTION(-1,
                    'Update',
                    'Update',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Accounts\UpdateSession.ascx',
                    v_V_EnableViewStateFalse,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Horizontal,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Used to update the session menus and roles.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Authenticated',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   -- Under Maintance
   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'UnderMaintance';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuSystem Administration';

   ZFP_SET_FUNCTION(-1,
                    'Under Maintance',
                    'Under Maintance',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Administration\UnderMaintance.ascx',
                    v_V_EnableViewStateFalse,
                    v_V_IsNavFalse,
                    v_V_NAV_TYPE_Horizontal,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Used to update the session menus and roles.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Anonymous,Authenticated',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding AlwaysLogon');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Security';

   v_V_MyAction := 'AlwaysLogon';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuSystem Administration';

   ZFP_SET_FUNCTION(-1,
                    'Always Logon',
                    'Always Logon',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'none',
                    v_V_EnableViewStateFalse,
                    v_V_IsNavFalse,
                    v_V_NAV_TYPE_Horizontal,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Used to update the session menus and roles.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'AlwaysLogon',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding Edit DB Information');

   SELECT FUNCTION_TYPE_SEQ_ID
     INTO v_V_FUNCTION_TYPE
     FROM ZFC_FUNCTION_TYPES
      WHERE NAME = 'Module';

   v_V_MyAction := 'Edit DB Information';

   SELECT FUNCTION_SEQ_ID
     INTO v_V_ParentID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = 'mnuSystem Administration';

   ZFP_SET_FUNCTION(-1,
                    'Edit DB Information',
                    'Edit DB Information',
                    v_V_FUNCTION_TYPE,
                    v_V_ALLOW_HTML_INPUT,
                    v_V_ALLOW_COMMENT_HTML_INPUT,
                    'Modules\System\Administration\Configuration\AddEditDBInformation.ascx',
                    v_V_EnableViewStateTrue,
                    v_V_IsNavTrue,
                    v_V_NAV_TYPE_Hierarchical,
                    v_V_MyAction,
                    v_V_ParentID,
                    'Used to update the ZF_Information table, enable inheritence.',
                    v_V_DeveloperID,
                    v_now,
                    v_V_DeveloperID,
                    v_now,
                    v_V_PRIMARY_KEY,
                    v_V_ErrorCode);

   SELECT function_seq_id
     INTO v_V_FunctionID
     FROM ZFC_FUNCTIONS
      WHERE ACTION = v_V_MyAction;

   ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                        1,
                        'Developer',
                        v_V_ViewPermission,
                        v_V_DeveloperID,
                        v_V_ErrorCode);

   DBMS_OUTPUT.PUT_LINE('Adding messages');

   ZFP_SET_MESSAGE(-1,
                   v_V_SE_SEQ_ID,
                   'Logon Error',
                   'Logon Error',
                   'Displayed when logon fails',
                   '<b>Invalid Account or Password!</b>',
                   v_V_FORMAT_AS_HTML_TRUE,
                   v_V_DeveloperID,
                   v_now,
                   v_V_DeveloperID,
                   v_now,
                   v_V_PRIMARY_KEY,
                   v_V_ErrorCode);

   ZFP_SET_MESSAGE(-1,
                   v_V_SE_SEQ_ID,
                   'New Account',
                   'New Account',
                   'Message sent when an account is created.',
                   'Dear <FullName>,

                   There has been a request for a new account:

                       Please Use this link to logon:
                    <Server>Default.aspx?Action=Logon&Account=<AccountName>&Password=<Password>

                   <b>Please note once you have logged on using this link you will only be able to change our password.</b>',
                   v_V_FORMAT_AS_HTML_FALSE,
                   v_V_DeveloperID,
                   v_now,
                   v_V_DeveloperID,
                   v_now,
                   v_V_PRIMARY_KEY,
                   v_V_ErrorCode);

   ZFP_SET_MESSAGE(-1,
                   v_V_SE_SEQ_ID,
                   'Request Password Reset UI',
                   'Request Password Reset UI',
                   'Displayed when new password is requested',
                   '<b>An EMail has been send to your account with instructions!</b>',
                   v_V_FORMAT_AS_HTML_TRUE,
                   v_V_DeveloperID,
                   v_now,
                   v_V_DeveloperID,
                   v_now,
                   v_V_PRIMARY_KEY,
                   v_V_ErrorCode);

   ZFP_SET_MESSAGE(-1,
                   v_V_SE_SEQ_ID,
                   'RequestNewPassword',
                   'Request New Password',
                   'Request New Password',
                   'Dear <FullName>,

                   There has been a request for a password change:

                       Please Use this link to logon:
                    <Server>Default.aspx?Action=Logon&Account=<AccountName>&Password=<Password>

                   <b>Please note once you have logged on using this link you will only be able to change our password.</b>',
                   v_V_FORMAT_AS_HTML_TRUE,
                   v_V_DeveloperID,
                   v_now,
                   v_V_DeveloperID,
                   v_now,
                   v_V_PRIMARY_KEY,
                   v_V_ErrorCode);

   ZFP_SET_MESSAGE(-1,
                   1,
                   'WebConfigNotSaved',
                   'Blank Environment Text',
                   'Blank Environment Text',
                   'Settings have not been saved.',
                   v_V_FORMAT_AS_HTML_FALSE,
                   v_V_DeveloperID,
                   v_now,
                   v_V_DeveloperID,
                   v_now,
                   v_V_PRIMARY_KEY,
                   v_V_ErrorCode);

   ZFP_SET_MESSAGE(-1,
                   1,
                   'WebConfigIsLocked',
                   'Web Config Is Locked',
                   'Web Config Is Locked',
                   'Configuration Section is locked. Unable to modify.',
                   v_V_FORMAT_AS_HTML_FALSE,
                   v_V_DeveloperID,
                   v_now,
                   v_V_DeveloperID,
                   v_now,
                   v_V_PRIMARY_KEY,
                   v_V_ErrorCode);

   ZFP_SET_MESSAGE(-1,
                   1,
                   'WebConfigEnvironmentRequired',
                   'Web Config Environment Required',
                   'Web Config Environment Required',
                   'You have selected a new environment but did not give the name.',
                   v_V_FORMAT_AS_HTML_FALSE,
                   v_V_DeveloperID,
                   v_now,
                   v_V_DeveloperID,
                   v_now,
                   v_V_PRIMARY_KEY,
                   v_V_ErrorCode);

   ZFP_SET_MESSAGE(-1,
                   1,
                   'ErrorAccountDetails',
                   'Error Account Details',
                   'Error Account Details',
                   'Could not set account details.',
                   v_V_FORMAT_AS_HTML_FALSE,
                   v_V_DeveloperID,
                   v_now,
                   v_V_DeveloperID,
                   v_now,
                   v_V_PRIMARY_KEY,
                   v_V_ErrorCode);

   ZFP_SET_MESSAGE(-1,
                   1,
                   'PasswordSendMailError',
                   'Password Send Mail Error',
                   'Password Send Mail Error',
                   'The password was reset, but, an email could not be sent.',
                   v_V_FORMAT_AS_HTML_FALSE,
                   v_V_DeveloperID,
                   v_now,
                   v_V_DeveloperID,
                   v_now,
                   v_V_PRIMARY_KEY,
                   v_V_ErrorCode);

   ZFP_SET_MESSAGE(-1,
                   1,
                   'DisabledAccount',
                   'Disabled Account',
                   'Disabled Account',
                   'This account is disabled.',
                   v_V_FORMAT_AS_HTML_FALSE,
                   v_V_DeveloperID,
                   v_now,
                   v_V_DeveloperID,
                   v_now,
                   v_V_PRIMARY_KEY,
                   v_V_ErrorCode);

   ZFP_SET_MESSAGE(-1,
                   1,
                   'SuccessChangePassword',
                   'Success Change Password',
                   'Success Change Password',
                   'Your password has been changed.',
                   v_V_FORMAT_AS_HTML_FALSE,
                   v_V_DeveloperID,
                   v_now,
                   v_V_DeveloperID,
                   v_now,
                   v_V_PRIMARY_KEY,
                   v_V_ErrorCode);

   ZFP_SET_MESSAGE(-1,
                   1,
                   'UnSuccessChangePassword',
                   'UnSuccess Change Password',
                   'UnSuccess ChangePassword',
                   'Your password has NOT been changed.',
                   v_V_FORMAT_AS_HTML_FALSE,
                   v_V_DeveloperID,
                   v_now,
                   v_V_DeveloperID,
                   v_now,
                   v_V_PRIMARY_KEY,
                   v_V_ErrorCode);

   ZFP_SET_MESSAGE(-1,
                   1,
                   'PasswordNotMatched',
                   'Password Not Matched',
                   'Password Not Matched',
                   'The OLD password did not match your current password.',
                   v_V_FORMAT_AS_HTML_FALSE,
                   v_V_DeveloperID,
                   v_now,
                   v_V_DeveloperID,
                   v_now,
                   v_V_PRIMARY_KEY,
                   v_V_ErrorCode);

   ZFP_SET_MESSAGE(-1,
                   1,
                   'UnderMaintance',
                   'Under Maintance',
                   'Under Maintance',
                   'The system is currently under maintance and logons have been limited.',
                   v_V_FORMAT_AS_HTML_FALSE,
                   v_V_DeveloperID,
                   v_now,
                   v_V_DeveloperID,
                   v_now,
                   v_V_PRIMARY_KEY,
                   v_V_ErrorCode);

   ZFP_SET_MESSAGE(-1,
                   1,
                   'UnderConstruction',
                   'Under Construction',
                   'Under Construction',
                   'The system is currently under construction.',
                   v_V_FORMAT_AS_HTML_FALSE,
                   v_V_DeveloperID,
                   v_now,
                   v_V_DeveloperID,
                   v_now,
                   v_V_PRIMARY_KEY,
                   v_V_ErrorCode);

   ZFP_SET_MESSAGE(-1,
                   1,
                   'NoDataFound',
                   'No Data Found',
                   'No Data Found',
                   'No Data Found.',
                   v_V_FORMAT_AS_HTML_FALSE,
                   v_V_DeveloperID,
                   v_now,
                   v_V_DeveloperID,
                   v_now,
                   v_V_PRIMARY_KEY,
                   v_V_ErrorCode);

   ZFP_SET_MESSAGE(-1,
                   1,
                   'ChangedSelectedSecurityEntity',
                   'Changed Selected Security Entity',
                   'Message for when a account changes the selected Security Entity.',
                   'You have changed your selected Security Entity.',
                   v_V_FORMAT_AS_HTML_FALSE,
                   v_V_DeveloperID,
                   v_now,
                   v_V_DeveloperID,
                   v_now,
                   v_V_PRIMARY_KEY,
                   v_V_ErrorCode);

   ZFP_SET_MESSAGE(-1,
                   1,
                   'SameAccountChangeAccount',
                   'Same Account Change Account',
                   'Message for when a account changes their own account.',
                   'showMSG("If you change your account the system will need to log you off.")',
                   v_V_FORMAT_AS_HTML_FALSE,
                   v_V_DeveloperID,
                   v_now,
                   v_V_DeveloperID,
                   v_now,
                   v_V_PRIMARY_KEY,
                   v_V_ErrorCode);

   -- Insert States
   DBMS_OUTPUT.PUT_LINE('Adding States');

   DELETE ZOP_STATES
   ;

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'AA', 'Armed Forces Americas', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'AE', 'Armed Forces Africa', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'AK', 'Alaska', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'AL', 'Alabama', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'AP', 'Armed Forces Pacific', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'AR', 'Arkansas', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'AS', 'American Samoa', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'AZ', 'Arizona', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'CA', 'California', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'CO', 'Colorado', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'CT', 'Connecticut', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'DC', 'District Of Columbia', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'DE', 'Delaware', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'FL', 'Florida', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'FM', 'Federated States of Micro', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'GA', 'Georgia', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'GU', 'Gaum', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'HI', 'Hawaii', v_V_ACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'IA', 'Iowa', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'ID', 'Idaho', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'IL', 'Illinois', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'IN', 'Indiana', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'KS', 'Kansas', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'KY', 'Kentucky', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'LA', 'Louisiana', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'MA', 'Massachusetts', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'MD', 'Maryland', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'ME', 'Maine', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'MH', 'Marshall Islands', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'MI', 'Michigan', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'MN', 'Minnesota', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'MO', 'Missouri', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'MP', 'Northern Mariana Islands', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'MS', 'Mississippi', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'MT', 'Montana', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'NC', 'North Carolina', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'ND', 'North Dakota', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'NE', 'Nebraska', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'NH', 'New Hampshire', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'NJ', 'New Jersey', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'NM', 'New Mexico', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'NV', 'Nevada', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'NY', 'New York', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'OH', 'Ohio', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'OK', 'Oklahoma', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'OR', 'Oregon', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'PA', 'Pennsylvania', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'PR', 'Puerto Rico', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'PW', 'Palau', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'RI', 'Rhode Island', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'SC', 'South Carolina', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'SD', 'South Dakota', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'TN', 'Tennessee', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'TX', 'Texas', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'UT', 'Utah', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'VA', 'Virginia', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'VI', 'Virgin Islands', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'VT', 'Vermont', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'WA', 'Washington', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'WI', 'Wisconsin', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'WV', 'West Virginia', v_V_INACTIVE );

   INSERT INTO ZOP_STATES
     ( STATE, DESCRIPTION, STATUS_SEQ_ID )
     VALUES ( 'WY', 'Wyoming', v_V_INACTIVE );

   NULL/*TODO:update statistics ZOP_STATES*/;

END;

