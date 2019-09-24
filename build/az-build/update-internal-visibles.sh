echo 'Updating internal visible references in files'
echo 'Locating InternalsVisibleTo References'
find . -name '*.cs' | xargs grep -l 'InternalsVisibleTo' > $AGENT_TEMPDIRECTORY/ivt.files
cat $AGENT_TEMPDIRECTORY/ivt.files

# Do an inital replace to preserve any existing Private key statements (this will ge replaced later)
# NOTE: This also adds a new line without the private key to ensure that the fake key can be added.
cat $AGENT_TEMPDIRECTORY/ivt.files | xargs sed -Ei  's/\s*(\[assembly:\s+InternalsVisibleTo\s*\()"(.*)\s*,\s*PublicKey=(.*)\)\]/\1* {\2} {\3}\r\n\[assembly: InternalsVisibleTo("\2"\)\]\r\n/g'

# Now the script can safely replace the reamining InternalsVisibleTo References as they should
# not contain a public key.
cat $AGENT_TEMPDIRECTORY/ivt.files | xargs sed -Ei "s/\s*(\[assembly:\s+InternalsVisibleTo\s*\()\"(.*)\s*\".*/#pragma warning disable S103\r\n\1\"\2, PublicKey=$DS_PK\"\)\]#pragma warning restore S103\r\n/g"

# Now we can reassemble the original Public Key inclusive InternalsVisibleTo statements
cat $AGENT_TEMPDIRECTORY/ivt.files | xargs sed -Ei "s/\s*(\[assembly:\s+InternalsVisibleTo\s*\()\*\s*\{(.*)\}\s*\{(.*)\}/\1\"\2, PublicKey=\3\)\]/g"

# dump changes to console for log
cat $AGENT_TEMPDIRECTORY/ivt.files | xargs git diff