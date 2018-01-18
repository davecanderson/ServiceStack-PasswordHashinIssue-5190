UPDATE `userauth`
SET 
	`PasswordHash` = 'lmXQNKAoWc+HXADy3U2yLd0lzactUDB1OqlUtd4PeugDJ06oKdVID82ellAbnShUPGhDibDW+SWU82sb702DqA==', 
	`DigestHa1Hash` = '94feafa0f52ba152208153d869c9fcbd', 
	`Salt` = 'aQWJVmQ='
WHERE `Id` = 2 AND `Email` = 'old@email.com';