use iTradeDatabase

-- insert the address types
Insert Into AddressTypes Values('Home');
Insert Into AddressTypes Values('Postal');
Insert Into AddressTypes Values('Business');

-- insert the phone types
Insert Into PhoneTypes Values('Home');
Insert Into PhoneTypes Values('Work');
Insert Into PhoneTypes Values('Business');
Insert Into PhoneTypes Values('Mobile');

-- insert the email types
Insert Into EmailTypes Values('Personal');
Insert Into EmailTypes Values('Work');
Insert Into EmailTypes Values('Business');

-- insert the socila network types
Insert Into SocialNetworkTypes Values('Facebook');
Insert Into SocialNetworkTypes Values('LinkedIn');
Insert Into SocialNetworkTypes Values('Twitter');
Insert Into SocialNetworkTypes Values('Instagram');

-- insert message types
Insert Into ProcessMessageTypes Values('Error');
Insert Into ProcessMessageTypes Values('Warning');
Insert Into ProcessMessageTypes Values('Success');
Insert Into ProcessMessageTypes Values('Information');

-- insert roles
Insert Into AspNetRoles Values(NEWID(), 'Admin of the application', 'Admin');
Insert Into AspNetRoles Values(NEWID(), 'User of the applciation', 'Trader');







