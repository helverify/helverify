#!/bin/sh

ethdir=/home/eth

pkill -HUP geth

echo y | geth removedb

sleep 5

rm -f $ethdir/data/geth.ipc

geth --datadir $ethdir/data --networkid 13337 --port $PORT --syncmode full --nat extip:`getent hosts host.docker.internal | cut -d' ' -f1` --http --http.addr "0.0.0.0" --http.api personal,eth,net,web3 --http.corsdomain https://remix.ethereum.org --txpool.accountslots="200000" --txpool.globalslots="200000" --allow-insecure-unlock --ws --ws.port 8546 --ws.addr="0.0.0.0" --ws.api eth,net,web3 --ws.origins "*" --rpc.gascap="50000000" --cache.trie.rejournal="0h30m0s" --cache 2048 --verbosity 4 --bootnodes= > $ethdir/eth.log 2>&1 &

until [ -e $ethdir/data/geth.ipc ]
do
    sleep 5
done

for element in $(jq '.nodes[]' $ethdir/nodes.json);
do
    geth attach --exec "admin.addPeer($element)" $ethdir/data/geth.ipc 
done

cp `ls $ethdir/data/keystore/UTC* | head -1` $ethdir/private.key
