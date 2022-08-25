#!/bin/sh

ethdir=/home/eth

pkill geth

geth --datadir $ethdir/data --networkid 13337 --port $PORT --syncmode full --miner.gastarget="180000000" --miner.gaslimit="180000000" --miner.gasprice="1" --nat extip:`dig +short host.docker.internal` --txpool.accountslots="20000" --txpool.globalslots="20000" --cache.trie.rejournal="0h30m0s" --cache 2048 --unlock `cat $ethdir/address` --password $ethdir/password --verbosity 4 --bootnodes= > $ethdir/eth.log 2>&1 &
