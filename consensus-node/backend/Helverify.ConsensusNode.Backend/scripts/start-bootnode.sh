#!/bin/sh

ethdir=/home/eth

geth -datadir $ethdir/data init $ethdir/genesis.json > $ethdir/eth.log 2>&1

geth --datadir $ethdir/data --nat extip:`dig +short host.docker.internal` --unlock 0 --password $ethdir/password --networkid 13337 --mine > $ethdir/eth.log 2>&1 &

until [ -e $ethdir/data/geth.ipc ]
do
    sleep 5
done

geth attach $ethdir/data/geth.ipc --exec admin.nodeInfo.enr > bootnode
