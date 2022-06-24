#!/bin/sh

ethdir=/home/eth

pkill geth

rm -rf $ethdir/data

rm $ethdir/address

rm $ethdir/nodes.json

rm $ethdir/genesis.json

mkdir -p $ethdir/data

echo $BC_ACCOUNT_PWD > $ethdir/password

geth --datadir $ethdir/data account new --password $ethdir/password

cat $ethdir/data/keystore/`ls $ethdir/data/keystore/ -t | head -n 1` | jq -r '.address' > $ethdir/address

# jq:https://unix.stackexchange.com/questions/551193/how-to-find-value-of-key-value-in-json-in-bash-script
# sorting: https://stackoverflow.com/questions/1015678/get-most-recent-file-in-a-directory-on-linux
