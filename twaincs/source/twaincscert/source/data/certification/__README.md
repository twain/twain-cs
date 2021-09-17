These are the scripts for doing the certification tests.  This folder will
be deleted and rewritten by the certify command, so don't put anything in
here you care about.

These have been designed such that it's possible to use the run command
on any "Certification.tc" file.

If doing diagnostic work, bear in mind that it may be helpful to go into
the "Certification.tc" files to comment out tests you're not interested
in running, or use a goto to skip over tests.

The 'echo' command is your friend.  You can learn a lot from it.  Or you
can use the 'verbose' option to get the program to hork out everything
going on.  The 'runv' command will show all of the TWAIN commands that
are issued during the test.
