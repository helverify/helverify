#!/bin/sh

ethdir=/home/eth

pkill -HUP geth

echo y | geth removedb

sleep 5

rm -f $ethdir/data/geth.ipc

geth --datadir $ethdir/data --networkid 13337 --port $PORT --syncmode full --nat extip:`dig +short host.docker.internal` --http --http.addr "0.0.0.0" --http.api personal,eth,net,web3 --http.corsdomain https://remix.ethereum.org --allow-insecure-unlock --ws --ws.port 8546 --ws.addr="0.0.0.0" --ws.api eth,net,web3 --ws.origins "*" --bootnodes= > $ethdir/eth.log 2>&1 &

until [ -e $ethdir/data/geth.ipc ]
do
    sleep 5
done

for element in $(jq '.nodes[]' $ethdir/nodes.json);
do
    geth attach $ethdir/data/geth.ipc --exec "admin.addPeer($element)"
done

cp `ls $ethdir/data/keystore/UTC* | head -1` $ethdir/private.key